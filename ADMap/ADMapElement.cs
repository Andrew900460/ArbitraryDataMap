

namespace SerialMap {

	// Basic type which holds the data for ADMap
	// Includes "ADMType" which tells us which type we are holding. So the data can be saved/read to file.

	public class ADMapElement {
		public ADMType dataType;
		public object data;
		public ADMapElement(ADMType type, object data) =>
			(dataType, this.data) = (type, data);
	}
}
