using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkSharp.Tasks
{
    public class Assign : IWorkflowTask
    {

        public IDictionary<string, dynamic> Definition { get; private set; }
        private Interpolator Interpolator { get; set; }
        private SelectHtmlNodes SelectNodes { get; set; }

        public string Name { get; private set; }
        public dynamic Expression { get; private set; }
        public dynamic FromNode { get; private set; }
        public dynamic Model { get; private set; }

        public Assign(Interpolator interpolator, SelectHtmlNodes selectNodes)
        {
            Interpolator = interpolator;
            SelectNodes = selectNodes;
        }


        public void InitializeFromJson(object definition)
        {
            Definition = (IDictionary<string, dynamic>)definition;
            Name = Definition["name"];
            Expression = Definition["expression"];

            FromNode = Definition.ContainsKey("fromNode") ? Definition["fromNode"] : null;
            Model = Definition.ContainsKey("model") ? Definition["model"] : null;
        }

        public async Task<object> Invoke(object context)
        {

            var evaluationContext = new ContextFrame { Scope = context, Step = this, };

            if (!(Expression is string))
            {
                // get result object
                //var resultObject = Interpolate(Name).Result;
                //var resultList = new List<dynamic>();

                try
                {
                    var dictionary = (IDictionary<string, dynamic>)Expression;
                    await AssignValueOnDynamic(Name, new System.Dynamic.ExpandoObject());
                    //context.Result = new System.Dynamic.ExpandoObject();

                    //var iss = new System.Dynamic.ExpandoObject();

                    foreach (var expressionItem in dictionary)
                    {
                        if(expressionItem.Value is string)
                        {
                            await AssignValue(expressionItem.Value, Name + "." + expressionItem.Key);

                        } else if(expressionItem.Value is object)
                        {
                            try
                            {
                                var dc = (IDictionary<string, dynamic>)expressionItem.Value;

                                dynamic selector;
                                dynamic expression;

                                if (dc.TryGetValue("selector", out selector)){

                                    var definition = new Dictionary<string, dynamic>() { {"from", FromNode },{ "selector", selector } };
                                    SelectNodes.InitializeFromJson(definition);
                                    var result = SelectNodes.Invoke(context).Result;


                                    if (dc.TryGetValue("expression", out expression)){

                                        var ResultContext = new Marshal() { Result = result };

                                        var expressionValue = await Interpolator.InterpolateExpression(expression, ResultContext);

                                        return await AssignValueOnDynamic(Name + "." + expressionItem.Key, expressionValue);

                                    }

                                }

                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }


                    }

                }
                catch (Exception)
                {

                    throw;
                }


            } else
            {
                await AssignValue(Expression, Name);
            }

            async Task<object> AssignValue(string expression, string name)
            {
                var expressionValue = await Interpolate(expression);
                return await AssignValueOnDynamic(name, expressionValue);
            }

            Task<object> Interpolate(string expression) => Interpolator.InterpolateExpression(expression, evaluationContext);
            Task<object> AssignValueOnDynamic(string name, dynamic expressionValue) => Interpolator.AssignValueOnDynamic(context, name, expressionValue);

            return Name;
           
        }
    }
}
