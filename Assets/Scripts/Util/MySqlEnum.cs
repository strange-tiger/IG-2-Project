namespace Asset {
	public enum ETableType {
		accountdb,
		characterdb,
		relationshipdb,
		requestdb,
		test,
	}
	public enum EaccountdbColumns {
		Email,
		Password,
		Nickname,
		Question,
		Answer,
	}
	public enum EcharacterdbColumns {
		Nickname,
		Gender,
		Tutorial,
		OnOff,
	}
	public enum ErelationshipdbColumns {
		UserA,
		UserB,
		Status,
		Timestamp,
	}
	public enum ErequestdbColumns {
		Requester,
		Respondent,
		Timestamp,
	}
	public enum EtestColumns {
		Data,
	}
}
