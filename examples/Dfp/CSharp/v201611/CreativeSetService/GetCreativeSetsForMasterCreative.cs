// Copyright 2016, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201611;
using Google.Api.Ads.Dfp.v201611;
using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201611 {
  /// <summary>
  /// This example gets all creative sets for a master creative.
  /// </summary>
  public class GetCreativeSetsForMasterCreative : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets all creative sets for a master creative.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    public static void Main() {
      GetCreativeSetsForMasterCreative codeExample = new GetCreativeSetsForMasterCreative();
      long masterCreativeId = long.Parse("INSERT_MASTER_CREATIVE_ID_HERE");
      Console.WriteLine(codeExample.Description);
      try {
        codeExample.Run(new DfpUser(), masterCreativeId);
      } catch (Exception e) {
        Console.WriteLine("Failed to get creative sets. Exception says \"{0}\"",
            e.Message);
      }
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="user">The DFP user object running the code example.</param>
    public void Run(DfpUser dfpUser, long masterCreativeId) {
      CreativeSetService creativeSetService =
          (CreativeSetService) dfpUser.GetService(DfpService.v201611.CreativeSetService);

      // Create a statement to select creative sets.
      int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
      StatementBuilder statementBuilder = new StatementBuilder()
          .Where("masterCreativeId = :masterCreativeId")
          .OrderBy("id ASC")
          .Limit(pageSize)
          .AddValue("masterCreativeId", masterCreativeId);

      // Retrieve a small amount of creative sets at a time, paging through until all
      // creative sets have been retrieved.
      int totalResultSetSize = 0;
      do {
        CreativeSetPage page = creativeSetService.getCreativeSetsByStatement(
            statementBuilder.ToStatement());

        // Print out some information for each creative set.
        if (page.results != null) {
          totalResultSetSize = page.totalResultSetSize;
          int i = page.startIndex;
          foreach (CreativeSet creativeSet in page.results) {
            Console.WriteLine(
                "{0}) Creative set with ID {1} and name \"{2}\" was found.",
                i++,
                creativeSet.id,
                creativeSet.name
            );
          }
        }

        statementBuilder.IncreaseOffsetBy(pageSize);
      } while (statementBuilder.GetOffset() < totalResultSetSize);

      Console.WriteLine("Number of results found: {0}", totalResultSetSize);
    }
  }
}
