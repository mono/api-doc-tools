namespace MyFramework.MyOtherNamespace {
	///<summary>Make sure the namespace in this assembly doesn't get 'dropped'</summary>
	public class MyOtherClass {
		public string MyProperty {get;set;}
		///<summary>Hello</summary>
		public float Hello(int value) {
			return 0.0f;
		}
		///<summary>Is it me you're looking for</summary>
		public float Hello(double value) {
			return (float)value;
		}
	}
}
