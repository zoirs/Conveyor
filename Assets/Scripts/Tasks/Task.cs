using UnityEngine;

public class Task {
    private ProductType productType;
    private int needCount;
    private int coast;
    private int progress;
    private CityController _customer;
   
    public Task(ProductType productType, int needCount, int coast) {
        this.productType = productType;
        this.needCount = needCount;
        this.coast = coast;
        this.progress = 0;
    }

    public int NeedCount {
        get { return needCount; }
    }

    public ProductType ProductType => productType;

    public CityController Customer {
        get { return _customer; }
        set {
            if (value == null) {
                progress = 0;
            }
            _customer = value;
        }
    }

    public void AddProgress() {
        progress++;
    }

    public int Progress => progress;

    public bool IsComplete() {
        return progress >= needCount;
    }

    public int Coast => coast;
}