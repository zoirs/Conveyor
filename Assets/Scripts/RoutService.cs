using System.Collections.Generic;

public class RoutService {
    
    Dictionary<int, Dictionary<int, List<int>>> all = new Dictionary<int, Dictionary<int, List<int>>>();

    public List<int> get(int areaFrom, int areaTo) {
        return all[areaFrom][areaTo];
    }

    public void init() {
        
    }

    public void put(int areaFrom, int areaTo) {
//        Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
//        dictionary.Add(areaTo);
//        all.Add(areaFrom, dictionary);
    }
}