using Discord.Attribute;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;

namespace Discord.Controller
{
    public static class EventController
    {
        /// <summary>
        /// To be executed by the Discord Events that use Ids.
        /// </summary>
        /// <typeparam name="CLASS">The class that should be looked in by the controller, defined by using <see cref="ExecuteController{T}(string[], string, object)"></see></typeparam>
        /// <typeparam name="ENUM">The base enum that will get separated and passed to the actual controller</typeparam>
        /// <param name="ids">Passed Ids by the event e.g e.Interaction.Data.CustomId</param>
        /// <param name="e">The passed Event args</param>
        public static void InvokeEventControllerClass<CLASS, ENUM>(string ids, object e)
        {
            string[] splittedIds = ids.Split(',');
            var category = Converter.InteractionFormatter.GetFirstEnumFromIdAndRemove<ENUM>(ref splittedIds);
            InvokeEventController<CLASS, ENUM>(category, splittedIds, e);
        }
        /// <summary>
        /// Invokes the event controller by passing over the given class and enum. This method takes a look at all the created Controllers and picks one that gets executed and invokes the correct method.
        /// </summary>
        /// <typeparam name="CLASS">Passed method from the controller invoker</typeparam>
        /// <typeparam name="ENUM">Passed enum from the controller invoker</typeparam>
        /// <param name="PassedEnum"></param>
        /// <param name="ids"></param>
        /// <param name="e"></param>
        private static void InvokeEventController<CLASS, ENUM>(ENUM PassedEnum, string[] ids, object e)
        {
            var type = typeof(CLASS);
            var methods = type.GetMethods();
            EventControllerAttribute<ENUM>? attribute;
            foreach (var method in methods)
            {
                attribute = method.GetCustomAttribute<EventControllerAttribute<ENUM>>();
                if (attribute is not null && EqualityComparer<ENUM>.Default.Equals(attribute.Event, PassedEnum))
                    method.Invoke(null, new object[] { ids, e });
            }
        }
        /// <summary>
        /// Method To be placed in a separate Class that will be passed into the <see cref="InvokeEventControllerClass"></see>
        /// For the EventControllerAttribute you have to pass the enum type e.g. of all categories and pass the enum value for a specific category. Finally you have to execute the controller and use the category's enum for the generic.
        /// </summary>
        /// <typeparam name="T">The enum of the category</typeparam>
        /// <param name="ids">The given Ids without category enum.</param>
        /// <param name="nspace">The namespace with all the required events. E.g. Project_Sipster.DiscordBarista.Function.Partnership.Events</param>
        /// <param name="e">The passed event from the <see cref="InvokeEventControllerClass"></see> - just needs to be put it as a param and passed over to the controller.</param>
        /// <param name="execAssembly">The executing assembly. Get it via <see cref="Assembly.GetExecutingAssembly"></see></param>
        public static void ExecuteController<T>(string[] ids, string nspace, object e, Assembly execAssembly)
        {
            var category = Converter.InteractionFormatter.GetFirstEnumFromId<T>(ref ids);
            var q = (from t in execAssembly.GetTypes()
                     where t.IsClass && t.Namespace == nspace
                     select t).ToList();
            List<MethodInfo> methods = new();
            foreach (var type in q)
            {
                methods.AddRange(type.GetMethods());
            }
            List<EventControllerAttribute<T>>? attribute;
            foreach (var method in methods)
            {
                attribute = method.GetCustomAttributes<EventControllerAttribute<T>>().ToList();
                foreach (var attr in attribute)
                {
                    if (EqualityComparer<T>.Default.Equals(attr.Event, category))
                        method.Invoke(null, new object[] { ids, e });
                }
            }
        }
    }
}
