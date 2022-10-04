using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Thry {
    public class ThryFileCreator {

        [MenuItem("Thry/ShaderUI/UI Creator Helper/Create Label From Properties", false, priority = 40)]
        public static void CreateLabelFromExisting()
        {
            string[] names = GetProperties();
            var propertiesWithLabels = GetPropertiesWithLabels();
            string data = "";
            foreach (KeyValuePair<string, string> k in propertiesWithLabels )
            {
                data += k.Key + ":=" + k.Value + "\n";
            }
            Save(data, "_label.txt");
        }
        [MenuItem("Thry/ShaderUI/UI Creator Helper/Create Label From Properties", true, priority = 40)]
        public static bool CreateLabelFromExistingValidate()
        {
            return ValidateSelection();
        }

        [MenuItem("Thry/ShaderUI/UI Creator Helper/Create Label + Locale From Properties (WIP)", false, priority = 40)]
        public static void CreateLabelLocaleFromExisting()
        {
            string[] names = GetProperties();
            var propertiesWithLabels = GetPropertiesWithLabels();
            string label_data = "";
            string locale_data = ",English\n";
            foreach (KeyValuePair<string, string> k in propertiesWithLabels )
            {
                string prop = k.Key;
                string label = k.Value;
                string annotations = "";
                string tooltip = "";
                if (label.Contains("--{"))
                {
                    if(label.StartsWith("--{"))
                    {
                        annotations = label.TrimStart(new char[] { '-', '{' } ).TrimEnd('}');
                        label = "";
                    }
                    else
                    {
                        string[] temp = label.TrimEnd('}').Split(new [] { "--{" }, System.StringSplitOptions.None);
                        label = temp[0];
                        annotations = temp[1];
                        if (annotations.Contains("tooltip:"))
                        {
                            string[] tooltipTemp = annotations.Split(',');
                            tooltip = tooltipTemp.Where(t => t.Contains("tooltip:")).Select(t => t).ToList()[0];
                            annotations = annotations.Replace(tooltip, "");
                            tooltip = tooltip.Replace("tooltip:", "");
                        }
                    }
                }
                label_data += prop + ":=locale::" + prop + "_text--{" + annotations + "tooltip:locale::" + prop + "_tooltip}\n";
                locale_data +=  prop + "_text," + label + "\n" +
                                prop + "_tooltip," + tooltip + "\n";
            }
            Save(label_data, "_label.txt");
            Save(locale_data, "_locale.csv");
        }
        [MenuItem("Thry/ShaderUI/UI Creator Helper/Create Label + Locale From Properties (WIP)", true, priority = 40)]
        public static bool CreateLabelLocaleFromExistingValidate()
        {
            return ValidateSelection();
        }

        [MenuItem("Thry/ShaderUI/UI Creator Helper/Create Label From Properties (Formatted)", false, priority = 40)]
        public static void CreateFormattedLabelFromExisting()
        {
            string[] names = GetProperties();
            var propertiesWithLabels = GetPropertiesWithLabels();
            string data = "";
            string line = "";
            string indent = "";
            foreach (KeyValuePair<string, string> k in propertiesWithLabels)
            {
                string prop = k.Key;
                string label = k.Value;
                string prefix = indent;
                string doNewline = "";

                if (prop.StartsWith("m_"))
                {
                    if (prop.StartsWith("m_start"))
                    {
                        prefix = indent;
                        doNewline = "\n";
                        indent += "    ";
                    }
                    else if ( prop.StartsWith("m_end"))
                    {
                        indent = new string(' ', System.Math.Max(indent.Length - 4, 0));
                        prefix = indent;
                    }
                    else  // Plain "m_"/main section
                    {
                        indent = new string(' ', System.Math.Max(indent.Length - 4, 0));
                        doNewline = "\n";
                        prefix = indent;
                        indent += "    ";
                    }
                }

                line = doNewline + prefix + prop + ":=" + label;
                data += line + "\n";
            }
            Save(data, "_label.txt");
        }
        [MenuItem("Thry/ShaderUI/UI Creator Helper/Create Label From Properties (Formatted)", true, priority = 40)]
        public static bool CreateFormattedLabelFromExistingValidate()
        {
            return ValidateSelection();
        }

        [MenuItem("Thry/ShaderUI/UI Creator Helper/Create Label + Locale From Properties (Formatted) (WIP)", false, priority = 40)]
        public static void CreateFormattedLabelLocaleFromExisting()
        {
            string[] names = GetProperties();
            var propertiesWithLabels = GetPropertiesWithLabels();
            string label_data = "";
            string locale_data = ",English\n";
            string indent = "";
            foreach (KeyValuePair<string, string> k in propertiesWithLabels )
            {
                string prop = k.Key;
                string label = k.Value;

                string annotations = "";
                string tooltip = "";

                if (label.Contains("--{"))
                {
                    if(label.StartsWith("--{"))
                    {
                        annotations = label.TrimStart(new char[] { '-', '{' } ).TrimEnd('}');
                        label = "";
                    }
                    else
                    {
                        string[] temp = label.TrimEnd('}').Split(new [] { "--{" }, System.StringSplitOptions.None);
                        label = temp[0];
                        annotations = temp[1];
                        if (annotations.Contains("tooltip:"))
                        {
                            string[] tooltipTemp = annotations.Split(',');
                            tooltip = tooltipTemp.Where(t => t.Contains("tooltip:")).Select(t => t).ToList()[0];
                            annotations = annotations.Replace(tooltip, "");
                            tooltip = tooltip.Replace("tooltip:", "");
                        }
                    }
                }

                string prefix = indent;
                string doNewline = "";

                if (prop.StartsWith("m_"))
                {
                    if (prop.StartsWith("m_start"))
                    {
                        prefix = indent;
                        doNewline = "\n";
                        indent += "    ";
                    }
                    else if ( prop.StartsWith("m_end"))
                    {
                        indent = new string(' ', System.Math.Max(indent.Length - 4, 0));
                        prefix = indent;
                    }
                    else  // Plain "m_"/main section
                    {
                        indent = new string(' ', System.Math.Max(indent.Length - 4, 0));
                        doNewline = "\n";
                        prefix = indent;
                        indent += "    ";
                    }
                }

                label_data += doNewline + prefix + prop + ":=locale::" + prop + "_text--{" + annotations + "tooltip:locale::" + prop + "_tooltip}\n";
                // Don't put formatting in the CSV
                locale_data += prop + "_text," + label + "\n" +
                               prop + "_tooltip," + tooltip + "\n";
            }
            Save(label_data, "_label.txt");
            Save(locale_data, "_locale.csv");
        }
        [MenuItem("Thry/ShaderUI/UI Creator Helper/Create Label + Locale From Properties (Formatted) (WIP)", true, priority = 40)]
        public static bool CreateFormattedLabelLocaleFromExistingValidate()
        {
            return ValidateSelection();
        }

        [MenuItem("Thry/ShaderUI/UI Creator Helper/Create Label Boiler", false, priority = 40)]
        public static void CreateLabel()
        {
            string[] names = GetProperties();
            string data = names.Aggregate("", (n1, n2) => n1 + n2 + ":=" + n2 + "--{tooltip:}\n");
            Save(data, "_label.txt");
        }
        [MenuItem("Thry/ShaderUI/UI Creator Helper/Create Label Boiler", true, priority = 40)]
        static bool CreateLabelVaildate()
        {
            return ValidateSelection();
        }

        [MenuItem("Thry/ShaderUI/UI Creator Helper/Create Label Boiler + Locale Boiler", false, priority = 40)]
        public static void CreateLabelLocale()
        {
            string[] names = GetProperties();
            string label_data = names.Aggregate("", (n1, n2) => n1 + 
            n2 + ":=locale::" + n2 + "_text--{tooltip:locale::" + n2 + "_tooltip}\n");
            string locale_data = names.Aggregate(",English\n", (n1, n2) => n1 +
            n2 + "_text," + n2 + "\n"+
            n2 + "_tooltip,\n");
            Save(label_data, "_label.txt");
            Save(locale_data, "_locale.csv");
        }
        [MenuItem("Thry/ShaderUI Creator Helper/Create Label Boiler + Locale Boiler", true, priority = 40)]
        static bool CreateLabelLocaleValidate()
        {
            return ValidateSelection();
        }

        private static bool ValidateSelection()
        {
            if (Selection.activeObject == null)
                return false;
            string path = AssetDatabase.GetAssetPath(Selection.activeObject).ToLower();
            return path.EndsWith(".shader");
        }

        private static Dictionary<string, string> GetPropertiesWithLabels()
        {
            Shader shader = (Shader)Selection.activeObject;
            Dictionary<string, string> propertiesWithLabels = new Dictionary<string, string>();
            List<string> propNames = MaterialEditor.GetMaterialProperties(new Material[] { new Material(shader) }).Select(p => p.name).ToList();
            List<string> propLabels = MaterialEditor.GetMaterialProperties(new Material[] { new Material(shader) }).Select(p => p.displayName).ToList();
            for (int i = 0; i < propNames.Count; i++)
            {
                propertiesWithLabels[propNames[i]] = propLabels[i];
            }

            return propertiesWithLabels;
        }

        private static string[] GetProperties()
        {
            Shader shader = (Shader)Selection.activeObject;
            return MaterialEditor.GetMaterialProperties(new Material[] { new Material(shader) }).Select(p => p.name).ToArray();
        }

        private static void Save(string data, string add_string)
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            path = Path.GetDirectoryName(path)+ "/"+ Path.GetFileNameWithoutExtension(path) + add_string;
            FileHelper.WriteStringToFile(data, path);
            AssetDatabase.Refresh();
            EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(path));
        }
    }
}