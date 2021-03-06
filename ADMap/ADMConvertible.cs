

namespace SerialMap {

	// Attach this interface to one of your classes to define how it will be serialized

	public interface ADMConvertible {
		ADMap SaveToMap(ADMap map = null);

		object LoadFromMap(ADMap map);
	}

	// These extension functions make it simple to convert (any) object which implements ADMConvertible
	// to be saved into a ADMap

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
