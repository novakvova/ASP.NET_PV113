import {Link} from "react-router-dom";
import {Button} from "antd";

const ProductListPage = () => {
    return (
      <>
          <h1>Список продуктів</h1>
          <Link to={"/products/create"}>
              <Button type="primary">
                  Додати
              </Button>
          </Link>

          
      </>
    );
}

export default ProductListPage;