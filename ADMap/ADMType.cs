using System;


namespace SerialMap {

	public class ADMType {
		public readonly int typeID;
		public readonly Type type;
		public readonly WriterFunction WriterFunction;
		public readonly ReaderFunction ReaderFunction;
		public ADMType(int id, Type type, WriterFunction writer, ReaderFunction reader) {
			(typeID, this.type, WriterFunction, ReaderFunction) = (id, type, writer, reader);
		}
	}

}
