public class ProductEntity {
    public ProductType _productType;

    public ProductEntity(ProductType productType) {
        _productType = productType;
    }

    public ProductType ProductType {
        get { return _productType; }
        set { _productType = value; }
    }
}