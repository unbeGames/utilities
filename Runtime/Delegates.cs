using UnityEngine;

namespace Unbegames.Services {	
	public delegate void StringHandler(string str);
	public delegate void TwoStringHandler(string str1, string str2);
	public delegate void TypeHandler(System.Type str);
	public delegate void IntHandler(int value);
	public delegate void UShortHandler(ushort value);
	public delegate void IntBoolHandler(int value, bool available);
	public delegate void IntUShortHandler(int value, ushort secondValue);
	public delegate void ShortHandler(short value);
	public delegate void LongHandler(long value);
	public delegate void ULongHandler(ulong value);
	public delegate void TwoULongHandler(ulong value, ulong second);
	public delegate void FloatHandler(float value);
	public delegate void DoubleHandler(double value);
	public delegate void DoubleBoolHandler(double value, bool b);
	public delegate void BoolHandler(bool value);
	public delegate void EventHandler();
	public delegate void TwoIntHandler(int amount, int change);
	public delegate void TwoUShortHandler(ushort amount, ushort change);
	public delegate void TwoDoubleHandler(double one, double two);
	public delegate void LongIntHandler(long amount, int change);
	public delegate void Vector3Handler(Vector3 vector);
	public delegate void QuaternionHandler(Quaternion q);
	public delegate void StringArrayHandler(string[] values);
	public delegate void ColorHandler(Color color);
	public delegate void IntStringHandler(int index, string name);	
}