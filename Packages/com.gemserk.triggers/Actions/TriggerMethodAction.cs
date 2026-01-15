namespace Gemserk.Triggers.Actions
{
	[TriggerEditor("Invoke Object Method")]
	public class TriggerMethodAction : TriggerAction
	{
		public TriggerMethodData methodData = new ();

		public override string GetObjectName()
		{
			if (!methodData.target)
				return "MethodAction not configured";
			return $"{methodData.target.name}/{methodData.componentName}.{methodData.methodName}()";
		}

		// public override string ActionName {
		// 	get {
		// 		if (methodData.target == null)
		// 			return "MethodAction not configured";
		// 		return string.Format ("{0}/{1}.{2}()", methodData.target.name, methodData.componentName, methodData.methodName);
		// 	}
		// }

		private void Awake()
		{
			methodData?.CacheMethod ();
		}

		public override ITrigger.ExecutionResult Execute(object activator = null)
		{
			methodData.InvokeAction ();
			// LogTriggerExecuted();
			return ITrigger.ExecutionResult.Completed;
		}
		
		// public override bool Perform (object activator)
		// {
		// 	methodData.InvokeAction ();
		// 	LogTriggerExecuted();
		// 	return true;
		// }

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