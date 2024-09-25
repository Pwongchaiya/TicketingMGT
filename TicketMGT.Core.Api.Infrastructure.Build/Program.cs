﻿using System.Collections.Generic;
using System.IO;
using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

namespace TicketMGT.Core.Api.Infrastructure.Build
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var aDotNetClient = new ADotNetClient();

            var githubPipeline = new GithubPipeline
            {
                Name = "Build & Test Ticket MGT",

                OnEvents = new Events
                {
                    Push = new PushEvent
                    {
                        Branches = ["main"]
                    },

                    PullRequest = new PullRequestEvent
                    {
                        Branches = ["main"]
                    }
                },

                Jobs = new Dictionary<string, Job>
                {
                    {
                      "build",
                      new Job
                      {
                          RunsOn = BuildMachines.Windows2022,

                          Steps =
                          [
                              new CheckoutTaskV2
                              {
                                  Name = "Check out"
                              },

                              new SetupDotNetTaskV1
                              {
                                  Name = "Setup .Net",

                                  TargetDotNetVersion = new TargetDotNetVersion
                                  {
                                      DotNetVersion = "8.0.100",
                                      IncludePrerelease = true
                                  }
                              },

                              new RestoreTask
                              {
                                  Name = "Restore"
                              },

                              new DotNetBuildTask
                              {
                                  Name = "Build"
                              },

                              new TestTask
                              {
                                  Name = "Test"
                              }
                          ]
                      }
                    }
                }
            };

            string buildScriptPath = "../../../../.github/workflows/dotnet.yml";
            string directoryPath = Path.GetDirectoryName(buildScriptPath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            aDotNetClient.SerializeAndWriteToFile(githubPipeline, path: buildScriptPath);
        }
    }
}