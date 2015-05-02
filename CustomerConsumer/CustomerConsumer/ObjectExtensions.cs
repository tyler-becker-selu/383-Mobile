using System;

namespace CustomerConsumer
{
	public class JavaHolder : Java.Lang.Object
	{
		public readonly object _instance;
		public JavaHolder (object instance)
		{
			_instance = instance;
		}
	}
		public static class ObjectExtensions
		{
			public static Game ToNetObject<Game>(this Java.Lang.Object value)
			{
				if (value == null)
					return default(Game);
			
				if (!(value is JavaHolder))
					throw new InvalidOperationException ("Unable to convert .NET object. Only Java.Lang.Object created with .ToJava()");

				Game retVal;
				try{retVal = (Game)((JavaHolder)value)._instance;}
				finally{value.Dispose ();}
				return retVal;
			}


		public static Java.Lang.Object ToJavaObject<Game>(this Game value)
		{
			if (Equals (value, default(Game)) && !typeof(Game).IsValueType)
				return null;

			var holder = new JavaHolder (value);

			return holder;
		}
	}
}


