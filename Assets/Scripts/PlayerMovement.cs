using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
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





    /// <summary>
    /// Controls the player's movement in virtual reality.
    /// </summary>


    /// <summary>
    /// The rate acceleration during movement.
    /// </summary>
    public float Acceleration = 0.1f;

    /// <summary>
    /// The rate of damping on movement.
    /// </summary>
    public float Damping = 0.3f;

    /// <summary>
    /// The rate of additional damping when moving sideways or backwards.
    /// </summary>
    public float BackAndSideDampen = 0.5f;

    /// <summary>
    /// The force applied to the character when jumping.
    /// </summary>
    public float JumpForce = 0.3f;

    /// <summary>
    /// The rate of rotation when using a gamepad.
    /// </summary>
    public float RotationAmount = 1.5f;

    /// <summary>
    /// The rate of rotation when using the keyboard.
    /// </summary>
    public float RotationRatchet = 45.0f;

    /// <summary>
    /// The player will rotate in fixed steps if Snap Rotation is enabled.
    /// </summary>
    [Tooltip("The player will rotate in fixed steps if Snap Rotation is enabled.")]
    public bool SnapRotation = true;

    /// <summary>
    /// [Deprecated] When enabled, snap rotation will happen about the guardian rather
    /// than the player/camera viewpoint.
    /// </summary>
    [Tooltip("[Deprecated] When enabled, snap rotation will happen about the center of the " +
        "guardian rather than the center of the player/camera viewpoint. This (legacy) " +
        "option should be left off except for edge cases that require extreme behavioral " +
        "backwards compatibility.")]
    public bool RotateAroundGuardianCenter = false;

    /// <summary>
    /// How many fixed speeds to use with linear movement? 0=linear control
    /// </summary>
    [Tooltip("How many fixed speeds to use with linear movement? 0=linear control")]
    public int FixedSpeedSteps;

    /// <summary>
    /// If true, reset the initial yaw of the player controller when the Hmd pose is recentered.
    /// </summary>
    public bool HmdResetsY = true;

    /// <summary>
    /// If true, tracking data from a child OVRCameraRig will update the direction of movement.
    /// </summary>
    public bool HmdRotatesY = true;

    /// <summary>
    /// Modifies the strength of gravity.
    /// </summary>
    public float GravityModifier = 0.379f;

    /// <summary>
    /// If true, each OVRPlayerController will use the player's physical height.
    /// </summary>
    public bool useProfileData = true;

    /// <summary>
    /// The CameraHeight is the actual height of the HMD and can be used to adjust the height of the character controller, which will affect the
    /// ability of the character to move into areas with a low ceiling.
    /// </summary>
    [NonSerialized]
    public float CameraHeight;

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
    public bool Teleported;

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
    public bool EnableLinearMovement = true;

    /// <summary>
    /// When true, user input will be applied to rotation. Set this to false whenever the player controller needs to ignore input for rotation.
    /// </summary>
    public bool EnableRotation = true;

    /// <summary>
    /// Rotation defaults to secondary thumbstick. You can allow either here. Note that this won't behave well if EnableLinearMovement is true.
    /// </summary>
    public bool RotationEitherThumbstick = false;
    public float InitialYRotation { get; private set; }

    protected CharacterController Controller = null;
    protected OVRCameraRig CameraRig = null;

    private Vector3 _moveThrottle = Vector3.zero;
    private OVRPose? _initialPose;
    private float _moveScale = 1.0f;
    private float _fallSpeed = 0.0f;
    private float _moveScaleMultiplier = 1.0f;
    private float _simulationRate = 60f;
    private bool _haltUpdateMovement = false;
    private bool _playerControllerEnabled = false;
    private bool isControllerRight;

    [SerializeField]
    private ControllerScrollButton _controllerScrollButton;

    private void Start()
    {
        // Add eye-depth as a camera offset from the player controller
        var p = CameraRig.transform.localPosition;
        p.z = OVRManager.profile.eyeDepth;
        CameraRig.transform.localPosition = p;

        _controllerScrollButton.SwitchController.AddListener(SwitchController);
    }

    

    private void Awake()
    {
        Controller = gameObject.GetComponent<CharacterController>();

        if (Controller == null)
            Debug.LogWarning("OVRPlayerController: No CharacterController attached.");

        // We use OVRCameraRig to set rotations to cameras,
        // and to be influenced by rotation
        OVRCameraRig[] CameraRigs = gameObject.GetComponentsInChildren<OVRCameraRig>();

        if (CameraRigs.Length == 0)
            Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
        else if (CameraRigs.Length > 1)
            Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
        else
            CameraRig = CameraRigs[0];

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

            if (CameraRig != null)
            {
                CameraRig.UpdatedAnchors -= UpdateTransform;
            }
            _playerControllerEnabled = false;
        }
    }

    private void Update()
    {
        if (!_playerControllerEnabled)
        {
            if (OVRManager.OVRManagerinitialized)
            {
                OVRManager.display.RecenteredPose += ResetOrientation;

                if (CameraRig != null)
                {
                    CameraRig.UpdatedAnchors += UpdateTransform;
                }
                _playerControllerEnabled = true;
            }
            else
                return;
        }
    }

    protected virtual void UpdateController()
    {
        if (useProfileData)
        {
            if (_initialPose == null)
            {
                // Save the initial pose so it can be recovered if useProfileData
                // is turned off later.
                _initialPose = new OVRPose()
                {
                    position = CameraRig.transform.localPosition,
                    orientation = CameraRig.transform.localRotation
                };
            }

            var p = CameraRig.transform.localPosition;
            if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.EyeLevel)
            {
                p.y = OVRManager.profile.eyeHeight - (0.5f * Controller.height) + Controller.center.y;
            }
            else if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.FloorLevel)
            {
                p.y = -(0.5f * Controller.height) + Controller.center.y;
            }
            CameraRig.transform.localPosition = p;
        }
        else if (_initialPose != null)
        {
            // Return to the initial pose if useProfileData was turned off at runtime
            CameraRig.transform.localPosition = _initialPose.Value.position;
            CameraRig.transform.localRotation = _initialPose.Value.orientation;
            _initialPose = null;
        }

        CameraHeight = CameraRig.centerEyeAnchor.localPosition.y;

        if (CameraUpdated != null)
        {
            CameraUpdated();
        }

        UpdateMovement();

        Vector3 moveDirection = Vector3.zero;

        float motorDamp = (1.0f + (Damping * _simulationRate * Time.deltaTime));

        _moveThrottle.x /= motorDamp;
        _moveThrottle.y = (_moveThrottle.y > 0.0f) ? (_moveThrottle.y / motorDamp) : _moveThrottle.y;
        _moveThrottle.z /= motorDamp;

        moveDirection += _moveThrottle * _simulationRate * Time.deltaTime;

        // Gravity
        if (Controller.isGrounded && _fallSpeed <= 0)
            _fallSpeed = ((Physics.gravity.y * (GravityModifier * 0.002f)));
        else
            _fallSpeed += ((Physics.gravity.y * (GravityModifier * 0.002f)) * _simulationRate * Time.deltaTime);

        moveDirection.y += _fallSpeed * _simulationRate * Time.deltaTime;


        if (Controller.isGrounded && _moveThrottle.y <= transform.lossyScale.y * 0.001f)
        {
            // Offset correction for uneven ground
            float bumpUpOffset = Mathf.Max(Controller.stepOffset, new Vector3(moveDirection.x, 0, moveDirection.z).magnitude);
            moveDirection -= bumpUpOffset * Vector3.up;
        }

        if (PreCharacterMove != null)
        {
            PreCharacterMove();
            Teleported = false;
        }

        Vector3 predictedXZ = Vector3.Scale((Controller.transform.localPosition + moveDirection), new Vector3(1, 0, 1));

        // Move contoller
        Controller.Move(moveDirection);
        Vector3 actualXZ = Vector3.Scale(Controller.transform.localPosition, new Vector3(1, 0, 1));

        if (predictedXZ != actualXZ)
            _moveThrottle += (actualXZ - predictedXZ) / (_simulationRate * Time.deltaTime);
    }

    public virtual void UpdateMovement()
    {
        //todo: enable for Unity Input System
#if ENABLE_LEGACY_INPUT_MANAGER
        if (_haltUpdateMovement)
            return;

        if (EnableLinearMovement)
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
            if (!Controller.isGrounded)
                _moveScale = 0.0f;

            _moveScale *= _simulationRate * Time.deltaTime;

            // Compute this for key movement
            float moveInfluence = Acceleration * 0.1f * _moveScale * _moveScaleMultiplier;

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
                _moveThrottle += ort * (transform.lossyScale.z * moveInfluence * BackAndSideDampen * Vector3.back);
            if (moveLeft)
                _moveThrottle += ort * (transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.left);
            if (moveRight)
                _moveThrottle += ort * (transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.right);

            moveInfluence = Acceleration * 0.1f * _moveScale * _moveScaleMultiplier;

#if !UNITY_ANDROID // LeftTrigger not avail on Android game pad

            if (!isControllerRight)
            {
                moveInfluence *= 1.0f + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
            }
            else
            {
                moveInfluence *= 1.0f + OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
            }
#endif

            if (!isControllerRight)
            {
                Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

                // If speed quantization is enabled, adjust the input to the number of fixed speed steps.
                if (FixedSpeedSteps > 0)
                {
                    primaryAxis.y = Mathf.Round(primaryAxis.y * FixedSpeedSteps) / FixedSpeedSteps;
                    primaryAxis.x = Mathf.Round(primaryAxis.x * FixedSpeedSteps) / FixedSpeedSteps;
                }

                if (primaryAxis.y > 0.0f)
                    _moveThrottle += ort * (primaryAxis.y * transform.lossyScale.z * moveInfluence * Vector3.forward);

                if (primaryAxis.y < 0.0f)
                    _moveThrottle += ort * (Mathf.Abs(primaryAxis.y) * transform.lossyScale.z * moveInfluence * BackAndSideDampen * Vector3.back);

                if (primaryAxis.x < 0.0f)
                    _moveThrottle += ort * (Mathf.Abs(primaryAxis.x) * transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.left);

                if (primaryAxis.x > 0.0f)
                    _moveThrottle += ort * (primaryAxis.x * transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.right);
            }
            else
            {
                Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

                // If speed quantization is enabled, adjust the input to the number of fixed speed steps.
                if (FixedSpeedSteps > 0)
                {
                    primaryAxis.y = Mathf.Round(primaryAxis.y * FixedSpeedSteps) / FixedSpeedSteps;
                    primaryAxis.x = Mathf.Round(primaryAxis.x * FixedSpeedSteps) / FixedSpeedSteps;
                }

                if (primaryAxis.y > 0.0f)
                    _moveThrottle += ort * (primaryAxis.y * transform.lossyScale.z * moveInfluence * Vector3.forward);

                if (primaryAxis.y < 0.0f)
                    _moveThrottle += ort * (Mathf.Abs(primaryAxis.y) * transform.lossyScale.z * moveInfluence * BackAndSideDampen * Vector3.back);

                if (primaryAxis.x < 0.0f)
                    _moveThrottle += ort * (Mathf.Abs(primaryAxis.x) * transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.left);

                if (primaryAxis.x > 0.0f)
                    _moveThrottle += ort * (primaryAxis.x * transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.right);
            }
        }
#endif
    }

    /// <summary>
    /// Invoked by OVRCameraRig's UpdatedAnchors callback. Allows the Hmd rotation to update the facing direction of the player.
    /// </summary>
    public void UpdateTransform(OVRCameraRig rig)
    {
        Transform root = CameraRig.trackingSpace;
        Transform centerEye = CameraRig.centerEyeAnchor;

        if (HmdRotatesY && !Teleported)
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
        if (HmdResetsY && !HmdRotatesY)
        {
            Vector3 euler = transform.rotation.eulerAngles;
            euler.y = InitialYRotation;
            transform.rotation = Quaternion.Euler(euler);
        }
    }

    private void SwitchController(bool isLeft)
    {
        isControllerRight = isLeft;
    }
}


