namespace Asset {
	public enum ETableType {
		accountdb,
		bettingdb,
		characterdb,
		petinventorydb,
		relationshipdb,
		requestdb,
		roomlistdb,
		Max,
	}
	public enum EaccountdbColumns {
		Email,
		Password,
		Nickname,
		Question,
		Answer,
		HaveCharacter,
		Max,
	}
	public enum EbettingdbColumns {
		NickName,
		BettingGold,
		BettingChampionNumber,
		HaveGold,
		Max,
	}
	public enum EcharacterdbColumns {
		Nickname,
		Gender,
		Tutorial,
		OnOff,
		AvatarData,
		AvatarColor,
		Gold,
		Max,
	}
	public enum EpetinventorydbColumns {
		Nickname,
		PetStatus,
		PetLevel,
		PetExp,
		PetAsset,
		PetSize,
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
	public enum EroomlistdbColumns {
		UserID,
		Password,
		DisplayName,
		RoomNumber,
		Max,
	}
}
