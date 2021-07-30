using Epic.OnlineServices;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.Logging;
using Epic.OnlineServices.Platform;
using System;
using System.Configuration;

namespace ConsoleApp1
{
    class Eos
    {
        public PlatformInterface Platform;
        public ConnectInterface Connect;

        /// <summary>
        /// Initialize Epic Online Services SDK
        /// </summary>
        public void Initialize()
        {
            var appSettings = ConfigurationManager.AppSettings;

            // Set these values as appropriate. For more information, see the Developer Portal documentation.
            string productName = "MyCSharpApplication";
            string productVersion = "1.0";
            string productId = ConfigurationManager.AppSettings["product_id"];
            string sandboxId = ConfigurationManager.AppSettings["sandbox_id"];
            string deploymentId = ConfigurationManager.AppSettings["deployment_id"];
            string clientId = ConfigurationManager.AppSettings["client_id"];
            string clientSecret = ConfigurationManager.AppSettings["client_secret"];

            var initializeOptions = new InitializeOptions()
            {
                ProductName = productName,
                ProductVersion = productVersion
            };

            var initializeResult = PlatformInterface.Initialize(initializeOptions);
            if (initializeResult != Result.Success)
            {
                throw new Exception("Failed to initialize platform: " + initializeResult);
            }

            // The SDK outputs lots of information that is useful for debugging.
            // Make sure to set up the logging interface as early as possible: after initializing.
            LoggingInterface.SetLogLevel(LogCategory.AllCategories, LogLevel.VeryVerbose);
            LoggingInterface.SetCallback((LogMessage logMessage) =>
            {
                Console.WriteLine(logMessage.Message);
            });

            var options = new Options()
            {
                ProductId = productId,
                SandboxId = sandboxId,
                DeploymentId = deploymentId,
                ClientCredentials = new ClientCredentials()
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                }
            };

            Platform = PlatformInterface.Create(options);
            if (Platform == null)
            {
                throw new Exception("Failed to create platform");
            }
        }

        /// <summary>
        /// Polls Epic Online Services SDK for updates
        /// </summary>
        public void Tick()
        {
            Platform.Tick();
        }

        /// <summary>
        /// Shutdown Epic Online Services SDK
        /// </summary>
        public void Shutdown()
        {
            Platform.Release();

            var shutdownResult = PlatformInterface.Shutdown();
            if (shutdownResult != Result.Success)
            {
                throw new Exception("Failed to shutdown platform: " + shutdownResult);
            }
        }

        /// <summary>
        /// Login external user
        /// </summary>
        public void Login(string accessToken, ExternalCredentialType type, Action<ProductUserId> callback)
        {
            Connect = Platform.GetConnectInterface();
            if (Connect == null)
            {
                throw new Exception("Failed to get connect interface");
            }

            var options = new LoginOptions
            {
                Credentials = new Credentials
                {
                    Token = accessToken,
                    Type = type,
                }
            };

            Connect.Login(options, null, (LoginCallbackInfo info) =>
            {
                if (info.ResultCode == Result.Success)
                {
                    callback(info.LocalUserId);
                }
                else if (info.ResultCode == Result.InvalidUser)
                {
                    CreateUser(info.ContinuanceToken, callback);
                }
                else
                {
                    throw new Exception($"ConnectInterface Login Error: {info.ResultCode}");
                }
            });
        }

        private void CreateUser(ContinuanceToken continuanceToken, Action<ProductUserId> callback)
        {
            var options = new CreateUserOptions
            {
                ContinuanceToken = continuanceToken
            };

            Connect.CreateUser(options, null, (CreateUserCallbackInfo info) =>
            {
                if (info.ResultCode != Result.Success)
                {
                    throw new Exception($"ConnectInterface CreateUser Error: {info.ResultCode}");
                }
                else
                {
                    callback(info.LocalUserId);
                }
            });
        }
    }
}
