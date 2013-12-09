public enum OpenNIJoints
	{
		USER_ID			= 0,

		HEAD			= 1,
		NECK			= 2,
		TORSO			= 3,
		WAIST			= 4,

		LEFT_COLLAR		= 5,
		LEFT_SHOULDER	= 6,
		LEFT_ELBOW		= 7,
		LEFT_WRIST		= 8,
		LEFT_HAND		= 9,
		LEFT_FINGERTIP	=10,

		RIGHT_COLLAR	=11,
		RIGHT_SHOULDER	=12,
		RIGHT_ELBOW		=13,
		RIGHT_WRIST		=14,
		RIGHT_HAND		=15,
		RIGHT_FINGERTIP	=16,

		LEFT_HIP		=17,
		LEFT_KNEE		=18,
		LEFT_ANKLE		=19,
		LEFT_FOOT		=20,

		RIGHT_HIP		=21,
		RIGHT_KNEE		=22,
		RIGHT_ANKLE		=23,
		RIGHT_FOOT		=24,

		Num_Bones
	}
	
	public enum OpenNIJointValues
	{
		POS_X = 0,
		POS_Y,
		POS_Z,
		POS_CONF,

		ROT_X,
		ROT_Y,
		ROT_Z,
		ROT_CONF,

		LOCROT_X,
		LOCROT_Y,
		LOCROT_Z,
		LOCROT_CONF,

		Num_Values
	}
	
	public enum MSKinectJoints
	{
		 Hip_Center = 0,
		 Spine,
		 Shoulder_Center,
		 Head,
		 Shoulder_Left,
		 Elbow_Left,
		 Wrist_Left,
		 Hand_Left,
		 Shoulder_Right,
		 Elbow_Right,
		 Wrist_Right,
		 Hand_Right,
		 Hip_Left,
		 Knee_Left,
		 Ankle_Left,
		 Foot_Left,
		 Hip_Right,
		 Knee_Right,
		 Ankle_Right,
		 Foot_Right,
		
		 Num_Bones
	}