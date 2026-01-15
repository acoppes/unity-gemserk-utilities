using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Gemserk.Triggers
{
	[Serializable]
	public class TriggerMethodData
	{
		public const BindingFlags DefaultBindingFlags = System.Reflection.BindingFlags.Instance |
		                                         System.Reflection.BindingFlags.Public |
		                                         System.Reflection.BindingFlags.DeclaredOnly;

		public static bool IsIntParameterMethod(Component c, string methodName)
		{
			if (string.IsNullOrEmpty (methodName))
				return false;
			return IsIntParameterMethod (c.GetType ().GetMethod (methodName, DefaultBindingFlags));
		}

		public static bool IsIntParameterMethod(MethodInfo method)
		{
			if (method == null)
				return false;
			return IsParameterMethod (method, typeof(int));
		}

		public static bool IsObjectParameterMethod(Component c, string methodName)
		{
			if (string.IsNullOrEmpty (methodName))
				return false;
			return IsObjectParameterMethod (c.GetType ().GetMethod (methodName, DefaultBindingFlags));
		}

		public static bool IsObjectParameterMethod(MethodInfo method)
		{
			if (method == null)
				return false;
			return IsParameterMethod (method, typeof(UnityEngine.Object));
		}

		public static bool IsStringParameterMethod(Component c, string methodName)
		{
			if (string.IsNullOrEmpty (methodName))
				return false;
			return IsStringParameterMethod (c.GetType ().GetMethod (methodName, DefaultBindingFlags));
		}

		public static bool IsStringParameterMethod(MethodInfo method)
		{
			if (method == null)
				return false;
			return IsParameterMethod (method, typeof(string));
		}

		public static bool IsFloatParameterMethod(Component c, string methodName)
		{
			if (string.IsNullOrEmpty (methodName))
				return false;
			return IsFloatParameterMethod (c.GetType ().GetMethod (methodName, DefaultBindingFlags));
		}

		public static bool IsFloatParameterMethod(MethodInfo method)
		{
			if (method == null)
				return false;
			return IsParameterMethod (method, typeof(float));
		}

		static bool IsParameterMethod(MethodInfo method, Type t)
		{
			return method.GetParameters ().Length == 1 
			       && method.GetParameters () [0].ParameterType == t; 	
		}

		public GameObject target;

		public string componentName;

		public string methodName;

		public int[] intParameters;

		public float[] floatParameters;

		public string[] stringParameters;

		public UnityEngine.Object[] objectParameters;

		[NonSerialized]
		Component component;

		[NonSerialized]
		MethodInfo method;

		[NonSerialized]
		MethodInfo methodIntParameter;

		[NonSerialized]
		MethodInfo methodFloatParameter;

		[NonSerialized]
		MethodInfo methodStringParameter;

		[NonSerialized]
		MethodInfo methodObjectParameter;

		public int IntParameter {
			get {
				if (intParameters == null || intParameters.Length == 0)
					return 0;
				return (int) intParameters [0];
			}
			set {
				ClearAllParameters();

				if (intParameters == null || intParameters.Length == 0)
					intParameters = new int[1];
			
				intParameters [0] = value; 
			}
		}

		public float FloatParameter {
			get {
				if (floatParameters == null || floatParameters.Length == 0)
					return 0;
				return (float) floatParameters [0];
			}
			set {
				ClearAllParameters();

				if (floatParameters == null || floatParameters.Length == 0)
					floatParameters = new float[1];

				floatParameters [0] = value; 
			}
		}

		public string StringParameter {
			get {
				if (stringParameters == null || stringParameters.Length == 0)
					return null;
				return stringParameters [0];
			}
			set {
				ClearAllParameters();

				if (stringParameters == null || stringParameters.Length == 0)
					stringParameters = new string[1];

				stringParameters [0] = value; 
			}
		}

		public UnityEngine.Object ObjectParameter {
			get {
				if (objectParameters == null || objectParameters.Length == 0)
					return null;
				return objectParameters [0];
			}
			set {
				ClearAllParameters();

				if (objectParameters == null || objectParameters.Length == 0)
					objectParameters = new UnityEngine.Object[1];

				objectParameters [0] = value; 
			}
		}


		public void ClearAllParameters()
		{
			intParameters = null;
			floatParameters = null;
			stringParameters = null;
			objectParameters = null;
		}

		public void Configure (GameObject target, string componentName, string methodName)
		{
			this.target = target;
			this.componentName = componentName;
			this.methodName = methodName;
		}

		public void CacheMethod()
		{
			if (!string.IsNullOrEmpty (componentName)) {
				component = target.GetComponent (componentName);

				if (!component) {
					throw new UnityException (string.Format("Failed to get component {0} from {1}", componentName, target.name));
				}

				if (component && !string.IsNullOrEmpty (methodName)) {

					var methods = component.GetType ().GetMethods (DefaultBindingFlags);

					var methodsWithName = methods.Where (m => m.Name.Equals (methodName)).ToList();

					if (methodsWithName.Count > 0) {
						methodIntParameter = methodsWithName.FirstOrDefault (m => IsParameterMethod(m, typeof(int)));

						if (methodIntParameter != null)
							return;

						methodFloatParameter = methodsWithName.FirstOrDefault (m => IsParameterMethod(m, typeof(float)));

						if (methodFloatParameter != null)
							return;

						methodStringParameter = methodsWithName.FirstOrDefault (m => IsParameterMethod(m, typeof(string)));

						if (methodStringParameter != null)
							return;

						methodObjectParameter = methodsWithName.FirstOrDefault (m => IsParameterMethod(m, typeof(UnityEngine.Object)));				

						if (methodObjectParameter != null)
							return;

						method = methodsWithName.First ();

						if (method != null)
							return;
					}

					throw new UnityException ($"Failed to cache method {methodName} from component {componentName}");
				}
			}
		}

		[NonSerialized]
		readonly object[] parameters = new object[1];

		public void InvokeAction()
		{
			if (method != null) {
				method.Invoke (component, null);
				return;
			}

			if (methodIntParameter != null) {
				parameters [0] = intParameters [0];
				methodIntParameter.Invoke (component, parameters);
				return;
			}

			if (methodFloatParameter != null) {
				parameters [0] = floatParameters[0];
				methodFloatParameter.Invoke (component, parameters);
				return;
			}

			if (methodStringParameter != null) {
				parameters [0] = stringParameters[0];
				methodStringParameter.Invoke (component, parameters);
				return;
			}

			if (methodObjectParameter != null) {
				parameters [0] = objectParameters [0];
				methodObjectParameter.Invoke (component, parameters);
				return;
			}
		}

		public bool InvokeCondition()
		{
			if (method != null)
				return (bool) method.Invoke (component, null);

			if (methodIntParameter != null) {
				parameters [0] = intParameters [0];
				return (bool) methodIntParameter.Invoke (component, parameters);
			}

			if (methodFloatParameter != null) {
				parameters [0] = floatParameters[0];
				return (bool) methodFloatParameter.Invoke (component, parameters);
			}

			if (methodStringParameter != null) {
				parameters [0] = stringParameters[0];
				return (bool) methodFloatParameter.Invoke (component, parameters);
			}

			if (methodObjectParameter != null) {
				parameters [0] = objectParameters [0];
				return (bool) methodObjectParameter.Invoke (component, parameters);
			}

			return false;
		}
	}
}
