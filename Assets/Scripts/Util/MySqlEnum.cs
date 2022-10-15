namespace Asset {
	public enum ETableType {
		accountdb,
		characterdb,
		relationshipdb,
		requestdb,
		test,
		Max,
	}
	public enum EaccountdbColumns {
		Email,
		Password,
		Nickname,
		Question,
		Answer,
		Max,
	}
	public enum EcharacterdbColumns {
		Nickname,
		Gender,
		Tutorial,
		OnOff,
		Max,
	}
	public enum ErelationshipdbColumns {
		UserA,
		UserB,
		State,
		Timestamp,
		Max,
	}
	public enum ErequestdbColumns {
		Requester,
		Respondent,
		Timestamp,
		Max,
	}
	public enum EtestColumns {
		Data,
		Max,
	}
}
