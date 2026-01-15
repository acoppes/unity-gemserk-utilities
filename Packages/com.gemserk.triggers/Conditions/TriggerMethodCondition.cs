namespace Gemserk.Triggers.Conditions
{
	[TriggerEditor("Evaluate Object Method")]
	public class TriggerMethodCondition : TriggerCondition
	{
		public TriggerMethodData methodData = new ();

		public override string GetObjectName()
		{
			if (!methodData.target)
				return "MethodCondition not configured";
			return $"{methodData.target.name}/Is{methodData.componentName}.{methodData.methodName}()";
		}

		// public override string ConditionName {
		// 	get {
		// 		if (methodData.target == null)
		// 			return "MethodCondition not configured";
		// 		return string.Format ("{0}/Is{1}.{2}()", methodData.target.name, methodData.componentName, methodData.methodName);
		// 	}
		// }

		private void Awake()
		{
			// base.Awake();
			methodData?.CacheMethod ();
		}

		public override bool Evaluate (object activator)
		{
			return methodData.InvokeCondition ();
		}

		// public override void Validate (TriggerValidationContext validationContext)
		// {
		// 	base.Validate (validationContext);
		//
		// 	validationContext.Validated("Object", methodData.target != null);
		// 	validationContext.Validated("Component", !string.IsNullOrEmpty(methodData.componentName));
		// 	validationContext.Validated("Method", !string.IsNullOrEmpty(methodData.methodName));
		// }
	}
}