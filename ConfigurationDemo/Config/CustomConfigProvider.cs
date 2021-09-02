using ConfigurationDemo.Config.Parser;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ConfigurationDemo.Config
{
    public class CustomConfigProvider : ConfigurationProvider
    {
        public override void Load()
        {
            var dict = new Dictionary<string, string>()
            {
                {"Test","123" }
            };

            var xml = @"
                <settings>
                    <Data.Setting>
                        <DefaultConnection>
                            <Connection.String>Test.Connection.String</Connection.String>
                            <Provider>SqlClient</Provider>
                        </DefaultConnection>
                        <Inventory>
                            <ConnectionString>AnotherTestConnectionString</ConnectionString>
                            <Provider>MySql</Provider>
                        </Inventory>
                        <numbers>
                            <value>3</value>
                        </numbers>
                    </Data.Setting>
                </settings>";

            var data = XmlConfigurationStringParser.Read(xml);

            Data = data;
        }
    }
}
