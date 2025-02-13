using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace BenjaminHamon.Solitaire.UnityClient.Editor
{
	public class ReflectionEditorContext : EditorContext
	{
		private class PropertyContext
		{
			public PropertyContext(Type type, PropertyInfo property, object obj, object oldValue, object newValue)
			{
				this.Type = type;
				this.Obj = obj;
				this.Property = property;
				this.OldValue = oldValue;
				this.NewValue = newValue;
			}

			public readonly Type Type;
			public readonly object Obj;
			public readonly PropertyInfo Property;
			public readonly object OldValue;
			public readonly object NewValue;
		}

		private readonly List<PropertyContext> propertyCollection = new List<PropertyContext>();

		private BuildTarget oldBuildTarget;
		public BuildTarget BuildTarget { get; set; }

		public void AddProperty(Type type, string propertyName, object obj, object newValue)
		{
			PropertyInfo property = type.GetProperty(propertyName);
			object oldValue = property.GetGetMethod().Invoke(obj, new object[] { });
			PropertyContext propertyContext = new PropertyContext(type, property, obj, oldValue, newValue);
			propertyCollection.Add(propertyContext);
		}

		public void RemoveProperty(object obj, string propertyName)
		{
			PropertyContext matchingProperty = propertyCollection.FirstOrDefault(
				propertyContext => Object.ReferenceEquals(propertyContext.Obj, obj) && (propertyContext.Property.Name == propertyName));

			if (matchingProperty != null)
			{
				propertyCollection.Remove(matchingProperty);
			}
		}

		public void Apply()
		{
			oldBuildTarget = EditorUserBuildSettings.activeBuildTarget;

			if (oldBuildTarget != BuildTarget)
			{
				UnityEngine.Debug.LogFormat("[EditorContext] Switching build target: '{0}' => '{1}'", oldBuildTarget, BuildTarget);
				bool success = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(BuildTarget), BuildTarget);

				if (success == false)
				{
					throw new ApplicationException("[EditorContext] Switching build target failed");
				}
			}

			foreach (PropertyContext propertyContext in propertyCollection)
			{
				propertyContext.Property.SetValue(propertyContext.Obj, propertyContext.NewValue);
			}

			// Ensure changes are actually saved
			// (possibly not necessary for the editor but it makes the changes appear in the actual settings file)
			AssetDatabase.Refresh();
		}

		public void Revert()
		{
			BuildTarget newBuildTarget = EditorUserBuildSettings.activeBuildTarget;

			foreach (PropertyContext propertyContext in propertyCollection)
			{
				propertyContext.Property.SetValue(propertyContext.Obj, propertyContext.OldValue);
			}

			// Ensure changes are actually saved
			// (possibly not necessary for the editor but it makes the changes appear in the actual settings file)
			AssetDatabase.Refresh();

			if (oldBuildTarget != newBuildTarget)
			{
				UnityEngine.Debug.LogFormat("[EditorContext] Switching build target: '{0}' => '{1}'", newBuildTarget, oldBuildTarget);
				bool success = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(oldBuildTarget), oldBuildTarget);

				if (success == false)
				{
					throw new ApplicationException("[EditorContext] Switching build target failed");
				}
			}
		}
	}
}
