//using System;
//using System.Collections.Generic;

//namespace InfHelper.Models.Enums;

//public static class ClassGuid
//{
//	public static Dictionary<Guid, string> Map => _map;
//	private readonly static Dictionary<Guid, string> _map = new(){
//		{ new Guid("5989fce8-9cd0-467d-8a6a-5419e31529d4"), "AudioProcessingObjects" },
//		{ new Guid("72631e54-78a4-11d0-bcf7-00aa00b7b32a"), "BatteryDevices" },
//		{ new Guid("53D29EF7-377C-4D14-864B-EB3A85769359"), "Biometric" },
//		{ new Guid("e0cbf06c-cd8b-4647-bb8a-263b43f0f974"), "Bluetooth" },
//		{ new Guid("ca3e7ab9-b4c3-4ae6-8251-579ef933890f"), "Camera" },
//		{ new Guid("4d36e965-e325-11ce-bfc1-08002be10318"), "CDROM" },
//		{ new Guid("4d36e967-e325-11ce-bfc1-08002be10318"), "DiskDrive" }, // Duplicate
//		{ new Guid("4d36e968-e325-11ce-bfc1-08002be10318"), "Display" }, // Duplicate
//		{ new Guid("e2f84ce7-8efa-411c-aa69-97454ca4cb57"), "Extension" },
//		{ new Guid("4d36e969-e325-11ce-bfc1-08002be10318"), "FDC" }, //dup
//		{ new Guid("4d36e969-e325-11ce-bfc1-08002be10318"), "FloppyDisk" }, //dup
//		{ new Guid("4d36e969-e325-11ce-bfc1-08002be10318"), "HDC" }, //dup
//		{ new Guid("745a17a0-74d3-11d0-b6fe-00a0c90f57da"), "HIDClass" },
//		{ new Guid("48721b56-6795-11d2-b1a8-0080c72e74a2"), "Dot4" },
//		{ new Guid("49ce6ac8-6f86-11d2-b1e5-0080c72e74a2"), "Dot4Print" },
//		{ new Guid("7ebefbc0-3200-11d2-b4c2-00a0C9697d07"), "61883" },
//		{ new Guid("c06ff265-ae09-48f0-812c-16753d7cba83"), "AVC" },
//		{ new Guid("d48179be-ec20-11d1-b6b8-00c04fa372a7"), "SBP2" },
//		{ new Guid("6bdd1fc1-810f-11d0-bec7-08002be2092f"), "1394" }, //dup
//		{ new Guid("6bdd1fc6-810f-11d0-bec7-08002be2092f"), "Image" }, //dup
//		{ new Guid("6bdd1fc5-810f-11d0-bec7-08002be2092f"), "Infrared" }, //dup
//		{ new Guid("4d36e96b-e325-11ce-bfc1-08002be10318"), "Keyboard" },
//		{ new Guid("ce5939ae-ebde-11d0-b181-0000f8753ec4"), "MediumChanger" },
//		{ new Guid("4d36e970-e325-11ce-bfc1-08002be10318"), "MTD" }, //dup
//		{ new Guid("4d36e96d-e325-11ce-bfc1-08002be10318"), "Modem" }, //dup
//		{ new Guid("4d36e96e-e325-11ce-bfc1-08002be10318"), "Monitor" },
//		{ new Guid("4d36e96f-e325-11ce-bfc1-08002be10318"), "Mouse" },
//		{ new Guid("4d36e971-e325-11ce-bfc1-08002be10318"), "Multifunction" },
//		{ new Guid("4d36e96c-e325-11ce-bfc1-08002be10318"), "Media" },
//		{ new Guid("50906cb8-ba12-11d1-bf5d-0000f805f530"), "MultiportSerial" },
//		{ new Guid("4d36e972-e325-11ce-bfc1-08002be10318"), "Net" },
//		{ new Guid("4d36e973-e325-11ce-bfc1-08002be10318"), "NetClient" },
//		{ new Guid("4d36e973-e325-11ce-bfc1-08002be10318"), "NetService" },
//		{ new Guid("4d36e973-e325-11ce-bfc1-08002be10318"), "NetTrans" },
//		{ new Guid("268c95a1-edfe-11d3-95c3-0010dc4050a5"), "SecurityAccelerator" },
//		{ new Guid("4d36e977-e325-11ce-bfc1-08002be10318"), "PCMCIA" },
//		{ new Guid("4d36e977-e325-11ce-bfc1-08002be10318"), "Ports" },
//		{ new Guid("4d36e977-e325-11ce-bfc1-08002be10318"), "Printer" },
//		{ new Guid("4658ee7e-f050-11d1-b6bd-00c04fa372a7"), "PNPPrinters" },
//		{ new Guid("50127dc3-0f36-415e-a6cc-4cb3be910b65"), "Processor" },
//		{ new Guid("4d36e97b-e325-11ce-bfc1-08002be10318"), "SCSIAdapter" },
//		{ new Guid("d94ee5d8-d189-4994-83d2-f68d7d41b0e6"), "Securitydevices" },
//		{ new Guid("5175d334-c371-4806-b3ba-71fd53c9258d"), "Sensor" },
//		{ new Guid("50dd5230-ba8a-11d1-bf5d-0000f805f530"), "SmartCardReader" },
//		{ new Guid("5c4c3332-344d-483c-8739-259e934c9cc8"), "SoftwareComponent" },
//		{ new Guid("75416e63-5912-4dfa-ae8f-3efaccaffb14"), "NvmeDisk" },
//		{ new Guid("71a27cdd-812a-11d0-bec7-08002be2092f"), "Volume" },
//		{ new Guid("4d36e97d-e325-11ce-bfc1-08002be10318"), "System" },
//		{ new Guid("4d36e97d-e325-11ce-bfc1-08002be10318"), "TapeDrive" },
//		{ new Guid("88BAE032-5A81-49f0-BC3D-A4FF138216D6"), "USBDevice" },
//		{ new Guid("25dbce51-6c8f-4a72-8a6d-b54c2b4fc835"), "WCEUSBS" },
//		{ new Guid("eec5ad98-8080-425f-922a-dabf3de3f69a"), "WPD" },
//	};	
//}
