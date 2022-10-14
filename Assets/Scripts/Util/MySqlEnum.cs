namespace Asset
{
	public enum ETableType
	{
		accountdb,
		characterdb,
		relationshipdb,
		test,
	}
	public enum EaccountdbColumns
	{
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
		Requester,
		Respondent,
		Status,
		Timestamp,
	}
	public enum EtestColumns {
		Data,
	}
}
