

namespace SerialMap {
	public interface ADMConvertible {
		ADMap SaveToMap(ADMap map = null);

		object LoadFromMap(ADMap map);
	}

	public static class ConverterExtension {
		public static ADMap GenerateMap(this ADMConvertible data) {
			return data.SaveToMap(new ADMap());
		}

		public static T GenerateFromMap<T>(this ADMap map) where T : ADMConvertible, new() {
			var obj = new T();
			obj.LoadFromMap(map);
			return obj;
		}
	}
}
