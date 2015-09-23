using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utils
{
    public static class FisiologiaDaClasse
    {

        #region Atributos

        private const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.GetField;       

        #endregion

        #region Métodos

        /// <summary>
        /// Lista o conteúdo de um objeto
        /// </summary>
        /// <param name="objeto">objeto alvo de estudo</param>
        public static void Anatomia(object objeto)
        {
            ResourceManager gerenciadoDeStrings = new ResourceManager("pt-br", Assembly.GetExecutingAssembly());

            if (objeto != null)
            {                
                gerenciadoDeStrings.GetString("--> ", CultureInfo.CurrentUICulture);
                Assembly assemblyDominio = Assembly.GetAssembly(objeto.GetType());
                Type tipo = assemblyDominio.GetType(objeto.GetType().FullName);
                PropertyInfo[] properties = tipo.GetProperties();
                
                gerenciadoDeStrings.GetString(String.Format(CultureInfo.CurrentCulture,"{0} : {1}", tipo.Name, tipo.FullName), CultureInfo.CurrentUICulture);

                foreach (var property in properties)
                {

                    PropertyInfo propInfo = tipo.GetProperty(property.Name);
                    object objetoAlvo = propInfo.GetValue(objeto, null);

                    if (property.ReflectedType.IsClass && property.PropertyType.Name != "String" && objetoAlvo != null && assemblyDominio.GetTypes().Contains(objetoAlvo.GetType()))
                    {
                        if (property.PropertyType.IsEnum)
                        {
                            //Console.WriteLine(" {0} : {1} = {2}", property.Name, property.PropertyType.Name, objetoAlvo.ToString());
                            gerenciadoDeStrings.GetString(String.Format(CultureInfo.CurrentCulture," {0} : {1} = {2}", property.Name, property.PropertyType.Name, objetoAlvo.ToString()), CultureInfo.CurrentUICulture);
                        }
                        else
                        {
                            //Console.WriteLine(" {0} : {1} = {2} ...", property.Name, property.PropertyType.Name, );
                            gerenciadoDeStrings.GetString(String.Format(CultureInfo.CurrentCulture," {0} : {1} = {2} ...", property.Name, property.PropertyType.Name), CultureInfo.CurrentUICulture);
                        }
                    }
                    else
                    {
                        if (objetoAlvo != null)
                        {
                            //Console.WriteLine(" {0} : {1} = {2}", property.Name, property.PropertyType.Name, objetoAlvo.ToString());
                            gerenciadoDeStrings.GetString(String.Format(CultureInfo.CurrentCulture," {0} : {1} = {2}", property.Name, property.PropertyType.Name, objetoAlvo.ToString()), CultureInfo.CurrentUICulture);
                        }
                        else
                        {
                            //Console.WriteLine(" {0} : {1} = {2} NULL", property.Name, property.PropertyType.Name, "NULL");
                            gerenciadoDeStrings.GetString(String.Format(CultureInfo.CurrentCulture, " {0} : {1} = {2} NULL", property.Name, property.PropertyType.Name), CultureInfo.CurrentUICulture);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Copia todos os valores de um objeto para outro
        /// </summary>
        /// <param name="tipoOrigem">O tipo do objeto de Origem, pode ser também uma Interface.</param>
        /// <param name="objetoOrigem">O nome do objeto de Origem</param>
        /// <param name="tipoDestino">O nome do tipo de Destino/param>
        /// <param name="objetoDestino">O nome do objeto de Destino</param>
        public static void GeraReplica(Type tipoOrigem, object objetoOrigem, Type tipoDestino, object objetoDestino)
        {
            if (tipoOrigem == null)
                throw new ArgumentNullException("tipoOrigem", "Nao pode ser nulo");

            if (objetoOrigem == null)
                throw new ArgumentNullException("objetoOrigem", "Nao pode ser nulo");

            if (tipoDestino == null)
                throw new ArgumentNullException("tipoDestino", "Nao pode ser nulo");

            if (objetoDestino == null)
                throw new ArgumentNullException("objetoDestino", "Nao pode ser nulo");
            //Não copia se for o mesmo objeto
            if (!ReferenceEquals(objetoOrigem, objetoDestino))
            {
                //Colhe todas os valores publicos em tipoDestino com getters e setters
                Dictionary<string, PropertyInfo> propsDestino = new Dictionary<string, PropertyInfo>();
                PropertyInfo[] propriedades = tipoDestino.GetProperties(flags);
                foreach (PropertyInfo propriedade in propriedades)
                {
                    propsDestino.Add(propriedade.Name, propriedade);
                }

                //Neste ponto, pega todas as propriedades públicas em tipoOrigem com Getters e Setters
                propriedades = tipoOrigem.GetProperties(flags);
                foreach (PropertyInfo propOrigem in propriedades)
                {
                    // If a propriedade matches in name and type, copy across
                    if (propsDestino.ContainsKey(propOrigem.Name))
                    {
                        PropertyInfo propDestino = propsDestino[propOrigem.Name];
                        if (propDestino.PropertyType == propOrigem.PropertyType)
                        {
                            object valor = propOrigem.GetValue(objetoOrigem, null);
                            propDestino.SetValue(objetoDestino, valor, null);
                        }
                    }
                }
            }
        }

        #endregion

    }
}
