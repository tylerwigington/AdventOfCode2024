namespace AdventOfCode2024.Problems;

public static class Day5
{
    public static async Task<int> Part1Async()
    {
        using var file = File.OpenText("./Inputs/day5.txt");
        var parseUpdates = false;

        Dictionary<int, List<int>> rules = [];
        Dictionary<int, List<int>> inverseRules = [];
        List<List<int>> updates = [];
        do
        {
            var line = await file.ReadLineAsync();
            if (string.IsNullOrEmpty(line))
            {
                parseUpdates = true;
                continue;
            }

            if (parseUpdates)
            {
                updates.Add(line!.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());
            }
            else
            {
                var rule = line!.Split("|", StringSplitOptions.RemoveEmptyEntries);
                if(!rules.ContainsKey(int.Parse(rule[0])))
                    rules.Add(int.Parse(rule[0]), [int.Parse(rule[1])]);
                else
                    rules[int.Parse(rule[0])].Add(int.Parse(rule[1]));
                
                if(!inverseRules.ContainsKey(int.Parse(rule[1])))
                    inverseRules.Add(int.Parse(rule[1]), [int.Parse(rule[0])]);
                else
                    inverseRules[int.Parse(rule[1])].Add(int.Parse(rule[0]));
            }
            
        } while(!file.EndOfStream);

        List<int> middleNumbers = [];
        foreach (var update in updates)
        {
            var middle = update.Count / 2;
            var valid = true;
            for (var i = 0; i < update.Count-1; i++)
            {
                var curr = update[i];
                rules.TryGetValue(curr, out var ruleList);
                inverseRules.TryGetValue(curr, out var inverseRuleList);
                
                var except = update[(i+1)..].Except(ruleList ?? []).ToList();
                var inverseIntersect = update[(i + 1)..].Intersect(inverseRuleList ?? []).ToList();
                if (except.Count == 0 && inverseIntersect.Count == 0) continue;
                
                valid = false;
                break;
            }
            if (!valid) continue;
            middleNumbers.Add(update[middle]);
        }

        return middleNumbers.Sum();
    }

    public static async Task<int> BetterPart1Async()
    {
        using var file = File.OpenText("./Inputs/day5.txt");
        var parseUpdates = false;
        
        var graph = new Dictionary<int, List<int>>();
        var updates = new List<List<int>>();
        do
        {
            var line = await file.ReadLineAsync();
            if (string.IsNullOrEmpty(line))
            {
                parseUpdates = true;
                continue;
            }

            if (parseUpdates)
            {
                updates.Add(line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());
            }
            else
            {
                var rule = line!.Split("|", StringSplitOptions.RemoveEmptyEntries).ToArray();
                if(!graph.ContainsKey(int.Parse(rule[0])))
                    graph.Add(int.Parse(rule[0]), []);
                graph[int.Parse(rule[0])].Add(int.Parse(rule[1]));
            }
        }while(!file.EndOfStream);

        return updates.Where(u => IsValidUpdate(u, graph)).Select(u => u[u.Count / 2]).Sum();
    }

    public static async Task<int> Part2Async()
    {
        using var file = File.OpenText("./Inputs/day5_test.txt");
        var parseUpdates = false;
        
        var graph = new Dictionary<int, List<int>>();
        var updates = new List<List<int>>();
        do
        {
            var line = await file.ReadLineAsync();
            if (string.IsNullOrEmpty(line))
            {
                parseUpdates = true;
                continue;
            }

            if (parseUpdates)
            {
                updates.Add(line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());
            }
            else
            {
                var rule = line!.Split("|", StringSplitOptions.RemoveEmptyEntries).ToArray();
                if(!graph.ContainsKey(int.Parse(rule[0])))
                    graph.Add(int.Parse(rule[0]), []);
                graph[int.Parse(rule[0])].Add(int.Parse(rule[1]));
            }
        }while(!file.EndOfStream);

        return updates.Select(u => ValidateAndFix(u, graph)).Sum();
    }

    private static bool IsValidUpdate(List<int> update, Dictionary<int, List<int>> graph)
    {
        Dictionary<int, int> positions = [];
        for (var i = 0; i < update.Count; i++)
            positions[update[i]] = i;
        
        foreach (var page in update)
        {
            if(!graph.TryGetValue(page, out _)) continue;
            foreach (var next in graph[page])
            {
                // If `next` appears before `page` in the update, it's invalid
                if (positions.ContainsKey(next) && positions[next] < positions[page])
                    return false;
            }
        }
        return true;
    }
    
    private static int ValidateAndFix(List<int> update, Dictionary<int, List<int>> graph)
    {
        Dictionary<int, int> positions = [];
        for (var i = 0; i < update.Count; i++)
            positions[update[i]] = i;

        var relevantRulesGraph = graph.Where(ruleSet => update.Contains(ruleSet.Key)).ToDictionary();
        var fixedCount = 0;
        var pageIndex = 0;
        var pages = update.ToArray();
        while (pageIndex < update.Count)
        {
            var page = pages[pageIndex];
            if (!relevantRulesGraph.TryGetValue(page, out _))
            {
                pageIndex++;
                continue;
            };
            
            var fixedPage = false;
            foreach (var next in relevantRulesGraph[page])
            {
                // If `next` appears before `page` in the update, it's invalid
                if (positions.TryGetValue(next, out var nextIndex) && nextIndex < positions[page])
                {
                    positions[next] = positions[page];
                    positions[page] = nextIndex;
                    pages[nextIndex] = pages[pageIndex];
                    pages[pageIndex] = next;
                    fixedCount++;
                    fixedPage = true;
                };
            }
            pageIndex += fixedPage ? 0 : 1;
        }
        var updated = positions.OrderBy(p => p.Value).Select(p => p.Key).ToList();
        return fixedCount != 0 ? updated[updated.Count / 2] : 0;
    }

    public static int TopoSort(List<int> vertices, Dictionary<int, List<int>> graph)
    {
        var subGraph = graph.Where(ruleSet => vertices.Contains(ruleSet.Key)).ToDictionary();
        return 1;
    }
}