using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerMove : MonoBehaviourPun
{
    #region Obsoleted
    //private PlayerInput _playerInput;
    //private CharacterController _controller;
    //private Transform _cam;

    //private float _speed = 3.0f;

    //private void Start()
    //{
    //    _controller = GetComponent<CharacterController>();
    //    _playerInput = GetComponent<PlayerInput>();
    //    _cam = Camera.main.GetComponent<Transform>();
    //}

    //private void Update()
    //{
    //    MovePlayer();
    //}
    //private void MovePlayer()
    //{
    //    if (_playerInput.IsMove)
    //    {
    //        Vector3 playerLook = _cam.TransformDirection(Vector3.forward);

    //        _controller.SimpleMove(playerLook * _speed);
    //    }
    //}
    #endregion

    /// <summary>
    /// The rate acceleration during movement.
    /// </summary>
    [SerializeField] float _acceleration = 0.1f;

    /// <summary>
    /// The rate of damping on movement.
    /// </summary>
    [SerializeField] float _damping = 0.3f;

    /// <summary>
    /// The rate of additional damping when moving sideways or backwards.
    /// </summary>
    [SerializeField] float _backAndSideDampen = 0.5f;

    /// <summary>
    /// How many fixed speeds to use with linear movement? 0=linear control
    /// </summary>
    [Tooltip("How many fixed speeds to use with linear movement? 0=linear control")]
    [SerializeField] int _fixedSpeedSteps;

    /// <summary>
    /// If true, reset the initial yaw of the player controller when the Hmd pose is recentered.
    /// </summary>
    [SerializeField] bool _hmdResetsY = true;

    /// <summary>
    /// If true, tracking data from a child OVRCameraRig will update the direction of movement.
    /// </summary>
    [SerializeField] bool _hmdRotatesY = true;

    /// <summary>
    /// Modifies the strength of gravity.
    /// </summary>
    [SerializeField] float _gravityModifier = 0.379f;

    /// <summary>
    /// If true, each OVRPlayerController will use the player's physical height.
    /// </summary>
    [SerializeField] bool _useProfileData = true;

    /// <summary>
    /// The CameraHeight is the actual height of the HMD and can be used to adjust the height of the character controller, which will affect the
    /// ability of the character to move into areas with a low ceiling.
    /// </summary>
    [NonSerialized]
    private float _cameraHeight;

    /// <summary>
    /// This event is raised after the character controller is moved. This is used by the OVRAvatarLocomotion script to keep the avatar transform synchronized
    /// with the OVRPlayerController.
    /// </summary>
    public event Action<Transform> TransformUpdated;

    /// <summary>
    /// This bool is set to true whenever the player controller has been teleported. It is reset after every frame. Some systems, such as
    /// CharacterCameraConstraint, test this boolean in order to disable logic that moves the character controller immediately
    /// following the teleport.
    /// </summary>
    [NonSerialized] // This doesn't need to be visible in the inspector.
    private bool _teleported;

    /// <summary>
    /// This event is raised immediately after the camera transform has been updated, but before movement is updated.
    /// </summary>
    public event Action CameraUpdated;

    /// <summary>
    /// This event is raised right before the character controller is actually moved in order to provide other systems the opportunity to
    /// move the character controller in response to things other than user input, such as movement of the HMD. See CharacterCameraConstraint.cs
    /// for an example of this.
    /// </summary>
    public event Action PreCharacterMove;

    /// <summary>
    /// When true, user input will be applied to linear movement. Set this to false whenever the player controller needs to ignore input for
    /// linear movement.
    /// </summary>
    [SerializeField] bool _enableLinearMovement = true;

    /// <summary>
    /// When true, user input will be applied to rotation. Set this to false whenever the player controller needs to ignore input for rotation.
    /// </summary>
    [SerializeField] bool _enableRotation = false;

    /// <summary>
    /// Rotation defaults to secondary thumbstick. You can allow either here. Note that this won't behave well if EnableLinearMovement is true.
    /// </summary>
    [SerializeField] bool _rotationEitherThumbstick = false;
    public float InitialYRotation { get; private set; }

    protected CharacterController _controller = null;
    protected OVRCameraRig _cameraRig = null;

    private InventoryUIManager _inventoryUIManager = new InventoryUIManager();
    private Vector3 _moveThrottle = Vector3.zero;
    private OVRPose? _initialPose;
    private float _moveScale = 1.0f;
    private float _fallSpeed = 0.0f;
    private float _moveScaleMultiplier = 1.0f;
    private float _simulationRate = 60f;
    private bool _haltUpdateMovement = false;
    private bool _playerControllerEnabled = false;
    private bool _isControllerRight;

    [SerializeField] ControllerScrollButton _controllerScrollButton;

    private void Start()
    {
        // Add eye-depth as a camera offset from the player controller
        var p = _cameraRig.transform.localPosition;
        p.z = OVRManager.profile.eyeDepth;
        _cameraRig.transform.localPosition = p;

        _controllerScrollButton.SwitchController.AddListener(SwitchController);
    }



    private void Awake()
    {
        // 이거 이렇게 써도 괜찮은가 모르겠슴다 ㅎㅎ
        _controllerScrollButton = GameObject.Find("ChangeController").GetComponent<ControllerScrollButton>();

        _controller = gameObject.GetComponent<CharacterController>();

        if (_controller == null)
            Debug.LogWarning("OVRPlayerController: No CharacterController attached.");

        // We use OVRCameraRig to set rotations to cameras,
        // and to be influenced by rotation
        OVRCameraRig[] CameraRigs = gameObject.GetComponentsInChildren<OVRCameraRig>();

        if (CameraRigs.Length == 0)
            Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
        else if (CameraRigs.Length > 1)
            Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
        else
            _cameraRig = CameraRigs[0];

        InitialYRotation = transform.rotation.eulerAngles.y;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        if (_playerControllerEnabled)
        {
            OVRManager.display.RecenteredPose -= ResetOrientation;

            if (_cameraRig != null)
            {
                _cameraRig.UpdatedAnchors -= UpdateTransform;
            }
            _playerControllerEnabled = false;
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (!_playerControllerEnabled)
            {
                if (OVRManager.OVRManagerinitialized)
                {
                    OVRManager.display.RecenteredPose += ResetOrientation;

                    if (_cameraRig != null)
                    {
                        _cameraRig.UpdatedAnchors += UpdateTransform;
                    }
                    _playerControllerEnabled = true;
                }
                else
                    return;
            }
        }
    }

    protected virtual void UpdateController()
    {
        if (_useProfileData)
        {
            if (_initialPose == null)
            {
                // Save the initial pose so it can be recovered if useProfileData
                // is turned off later.
                _initialPose = new OVRPose()
                {
                    position = _cameraRig.transform.localPosition,
                    orientation = _cameraRig.transform.localRotation
                };
            }

            var p = _cameraRig.transform.localPosition;
            if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.EyeLevel)
            {
                p.y = OVRManager.profile.eyeHeight - (0.5f * _controller.height) + _controller.center.y;
            }
            else if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.FloorLevel)
            {
                p.y = -(0.5f * _controller.height) + _controller.center.y;
            }
            _cameraRig.transform.localPosition = p;
        }
        else if (_initialPose != null)
        {
            // Return to the initial pose if useProfileData was turned off at runtime
            _cameraRig.transform.localPosition = _initialPose.Value.position;
            _cameraRig.transform.localRotation = _initialPose.Value.orientation;
            _initialPose = null;
        }

        _cameraHeight = _cameraRig.centerEyeAnchor.localPosition.y;

        if (CameraUpdated != null)
        {
            CameraUpdated();
        }

        UpdateMovement();

        Vector3 moveDirection = Vector3.zero;

        float motorDamp = (1.0f + (_damping * _simulationRate * Time.deltaTime));

        _moveThrottle.x /= motorDamp;
        _moveThrottle.y = (_moveThrottle.y > 0.0f) ? (_moveThrottle.y / motorDamp) : _moveThrottle.y;
        _moveThrottle.z /= motorDamp;

        moveDirection += _moveThrottle * _simulationRate * Time.deltaTime;

        // Gravity
        if (_controller.isGrounded && _fallSpeed <= 0)
            _fallSpeed = ((Physics.gravity.y * (_gravityModifier * 0.002f)));
        else
            _fallSpeed += ((Physics.gravity.y * (_gravityModifier * 0.002f)) * _simulationRate * Time.deltaTime);

        moveDirection.y += _fallSpeed * _simulationRate * Time.deltaTime;


        if (_controller.isGrounded && _moveThrottle.y <= transform.lossyScale.y * 0.001f)
        {
            // Offset correction for uneven ground
            float bumpUpOffset = Mathf.Max(_controller.stepOffset, new Vector3(moveDirection.x, 0, moveDirection.z).magnitude);
            moveDirection -= bumpUpOffset * Vector3.up;
        }

        if (PreCharacterMove != null)
        {
            PreCharacterMove();
            _teleported = false;
        }

        Vector3 predictedXZ = Vector3.Scale((_controller.transform.localPosition + moveDirection), new Vector3(1, 0, 1));

        // Move contoller
        _controller.Move(moveDirection);
        Vector3 actualXZ = Vector3.Scale(_controller.transform.localPosition, new Vector3(1, 0, 1));

        if (predictedXZ != actualXZ)
            _moveThrottle += (actualXZ - predictedXZ) / (_simulationRate * Time.deltaTime);
    }

    public virtual void UpdateMovement()
    {
        //todo: enable for Unity Input System
#if ENABLE_LEGACY_INPUT_MANAGER
        if (_haltUpdateMovement)
            return;

        if (_enableLinearMovement)
        {
            bool moveForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            bool moveBack = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

            bool dpad_move = false;

            if (OVRInput.Get(OVRInput.Button.DpadUp))
            {
                moveForward = true;
                dpad_move = true;
            }

            if (OVRInput.Get(OVRInput.Button.DpadDown))
            {
                moveBack = true;
                dpad_move = true;
            }

            _moveScale = 1.0f;

            if ((moveForward && moveLeft) || (moveForward && moveRight) ||
                (moveBack && moveLeft) || (moveBack && moveRight))
                _moveScale = 0.70710678f;

            // No positional movement if we are in the air
            if (!_controller.isGrounded)
                _moveScale = 0.0f;

            _moveScale *= _simulationRate * Time.deltaTime;

            // Compute this for key movement
            float moveInfluence = _acceleration * 0.1f * _moveScale * _moveScaleMultiplier;

            // Run!
            if (dpad_move || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                moveInfluence *= 2.0f;

            Quaternion ort = transform.rotation;
            Vector3 ortEuler = ort.eulerAngles;
            ortEuler.z = ortEuler.x = 0f;
            ort = Quaternion.Euler(ortEuler);

            if (moveForward)
                _moveThrottle += ort * (transform.lossyScale.z * moveInfluence * Vector3.forward);
            if (moveBack)
                _moveThrottle += ort * (transform.lossyScale.z * moveInfluence * _backAndSideDampen * Vector3.back);
            if (moveLeft)
                _moveThrottle += ort * (transform.lossyScale.x * moveInfluence * _backAndSideDampen * Vector3.left);
            if (moveRight)
                _moveThrottle += ort * (transform.lossyScale.x * moveInfluence * _backAndSideDampen * Vector3.right);

            moveInfluence = _acceleration * 0.1f * _moveScale * _moveScaleMultiplier;

#if !UNITY_ANDROID // LeftTrigger not avail on Android game pad

            if (!_isControllerRight)
            {
                moveInfluence *= 1.0f + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
            }
            else
            {
                moveInfluence *= 1.0f + OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
            }
#endif

            if (!_isControllerRight)
            {
                Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

                // If speed quantization is enabled, adjust the input to the number of fixed speed steps.
                if (_fixedSpeedSteps > 0)
                {
                    primaryAxis.y = Mathf.Round(primaryAxis.y * _fixedSpeedSteps) / _fixedSpeedSteps;
                    primaryAxis.x = Mathf.Round(primaryAxis.x * _fixedSpeedSteps) / _fixedSpeedSteps;
                }

                if (primaryAxis.y > 0.0f)
                    _moveThrottle += ort * (primaryAxis.y * transform.lossyScale.z * moveInfluence * Vector3.forward);

                if (primaryAxis.y < 0.0f)
                    _moveThrottle += ort * (Mathf.Abs(primaryAxis.y) * transform.lossyScale.z * moveInfluence * _backAndSideDampen * Vector3.back);

                if (primaryAxis.x < 0.0f)
                    _moveThrottle += ort * (Mathf.Abs(primaryAxis.x) * transform.lossyScale.x * moveInfluence * _backAndSideDampen * Vector3.left);

                if (primaryAxis.x > 0.0f)
                    _moveThrottle += ort * (primaryAxis.x * transform.lossyScale.x * moveInfluence * _backAndSideDampen * Vector3.right);
            }
            else
            {
                Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

                // If speed quantization is enabled, adjust the input to the number of fixed speed steps.
                if (_fixedSpeedSteps > 0)
                {
                    primaryAxis.y = Mathf.Round(primaryAxis.y * _fixedSpeedSteps) / _fixedSpeedSteps;
                    primaryAxis.x = Mathf.Round(primaryAxis.x * _fixedSpeedSteps) / _fixedSpeedSteps;
                }

                if (primaryAxis.y > 0.0f)
                    _moveThrottle += ort * (primaryAxis.y * transform.lossyScale.z * moveInfluence * Vector3.forward);

                if (primaryAxis.y < 0.0f)
                    _moveThrottle += ort * (Mathf.Abs(primaryAxis.y) * transform.lossyScale.z * moveInfluence * _backAndSideDampen * Vector3.back);

                if (primaryAxis.x < 0.0f)
                    _moveThrottle += ort * (Mathf.Abs(primaryAxis.x) * transform.lossyScale.x * moveInfluence * _backAndSideDampen * Vector3.left);

                if (primaryAxis.x > 0.0f)
                    _moveThrottle += ort * (primaryAxis.x * transform.lossyScale.x * moveInfluence * _backAndSideDampen * Vector3.right);
            }
        }
#endif
    }

    /// <summary>
    /// Invoked by OVRCameraRig's UpdatedAnchors callback. Allows the Hmd rotation to update the facing direction of the player.
    /// </summary>
    public void UpdateTransform(OVRCameraRig rig)
    {
        Transform root = _cameraRig.trackingSpace;
        Transform centerEye = _cameraRig.centerEyeAnchor;

        if (_hmdRotatesY && !_teleported)
        {
            Vector3 prevPos = root.position;
            Quaternion prevRot = root.rotation;

            transform.rotation = Quaternion.Euler(0.0f, centerEye.rotation.eulerAngles.y, 0.0f);

            root.position = prevPos;
            root.rotation = prevRot;
        }

        UpdateController();
        if (TransformUpdated != null)
        {
            TransformUpdated(root);
        }
    }

    /// <summary>
    /// Resets the player look rotation when the device orientation is reset.
    /// </summary>
    public void ResetOrientation()
    {
        if (_hmdResetsY && !_hmdRotatesY)
        {
            Vector3 euler = transform.rotation.eulerAngles;
            euler.y = InitialYRotation;
            transform.rotation = Quaternion.Euler(euler);
        }
    }

    private void SwitchController(bool isLeft)
    {
        _isControllerRight = isLeft;
    }
}


