using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodREST.Extensions.SwaggerExtension
{
    public class Swagger
    {
        public string swagger { get; set; }
        public info info { get; set; }
        public string host { get; set; }
        public string basePath { get; set; }
        public IEnumerable<IDictionary<string, object>> tags { get; set; }
        public IEnumerable<string> schemes { get; set; }
        public IDictionary<string, IDictionary<string, pathDescription>> paths { get; set; }
        public IDictionary<string, securityDefinitionInfo> securityDefinition { get; set; }
        public IDictionary<string, IDictionary<string, object>> definitions { get; set; }
        public externalDocs externalDocs { get; set; }

        public void AddSecurityDefinition(string security_name, securityDefinitionInfo definition)
        {

            if (securityDefinition == null) { securityDefinition = new Dictionary<string, securityDefinitionInfo>(); }
            if (!securityDefinition.ContainsKey(security_name))
            {
                securityDefinition.Add(security_name, null);
            }
            securityDefinition[security_name] = definition;
        }

        public void AddTag(tag tag)
        {
            if (tags == null) { tags = new List<IDictionary<string, object>>(); }
            var list = tags as List<IDictionary<string, object>>;
            var newElemenet = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(tag.name)) { newElemenet.Add("name", tag.name); }
            if (!string.IsNullOrWhiteSpace(tag.description)) { newElemenet.Add("description", tag.description); }
            if (tag.externalDocs != null) { newElemenet.Add("externalDocs", tag.externalDocs); }


            list.Add(newElemenet);
        }

        public void AddOperation(string path, string verb, pathDescription description)
        {
            if (paths == null) { paths = new Dictionary<string, IDictionary<string, pathDescription>>(); };
            if (!paths.ContainsKey(path))
            {
                paths.Add(path, new Dictionary<string, pathDescription>());
            }
            var descriptor = paths[path];
            if (!descriptor.ContainsKey(verb))
            {
                descriptor.Add(verb, description);
            }
            else
            {
                descriptor[verb] = description;
            }
        }

        public void AddObjectDefinition(string definitionName, objectDefiniton objectDefinition)
        {
            if (definitions == null) { definitions = new Dictionary<string, IDictionary<string,object>>(); };
            if (!definitions.ContainsKey(definitionName))
            {
                definitions.Add(definitionName, new Dictionary<string, object>());
            }
            var def = definitions[definitionName];

            if (!def.ContainsKey("type"))
            {
                def.Add("type", objectDefinition.type);
            }
            if (!def.ContainsKey("required"))
            {
                def.Add("required", objectDefinition.RequiredProperties);
            }

            if (!def.ContainsKey("properties"))
            {
                var properties = new Dictionary<string, object>();
                foreach (var prop in objectDefinition.properties)
                {
                    properties.Add(prop.name, prop.propertyDescription);
                }
                def.Add("properties", properties);
            }
        }
    }
}
