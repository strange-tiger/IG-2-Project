namespace Asset {
	public enum ETableType {
		accountdb,
		bettingamountdb,
		bettingdb,
		characterdb,
		petinventorydb,
		relationshipdb,
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
		IsOnline,
		Max,
	}
	public enum EbettingamountdbColumns {
		OneAmount,
		TwoAmount,
		ThreeAmount,
		FourAmount,
		Amount,
		Max,
	}
	public enum EbettingdbColumns {
		Nickname,
		BettingGold,
		BettingChampionNumber,
		HaveGold,
		Max,
	}
	public enum EcharacterdbColumns {
		Nickname,
		Gender,
		Tutorial,
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
	public enum EroomlistdbColumns {
		UserID,
		Password,
		DisplayName,
		RoomNumber,
		Max,
	}
}
