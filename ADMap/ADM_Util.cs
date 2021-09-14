using System;

namespace SerialMap {
	using static ADM_Common;

	public class ADM_Util {

		public static ADMType GetADMTypeFromObject(object obj) {
			if(obj is ADMConvertible)
				return GetADMType(typeof(ADMap));
			else
				return GetADMType(obj.GetType());
		}

		public static ADMType GetADMType(Type type) => ADMTypesList[type];
		public static ADMType GetADMType<T>() => ADMTypesList[typeof(T)];
		public static ADMType GetADMType(int typeID) => ADMTypesByTypeID[typeID];
		
	}
}
