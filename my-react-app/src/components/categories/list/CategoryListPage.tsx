import {Card, Col, Row} from "antd";
import {useEffect, useState} from "react";
import {ICategoryItem} from "../../../interfaces/categories";
import http_common from "../../../http_common.ts";
import {APP_ENV} from "../../../env";

const CategoryListPage = () => {
    const { Meta } = Card;

    const [list, setList] = useState<ICategoryItem[]>([]);

    useEffect(() => {
        http_common.get<ICategoryItem[]>("/api/categories")
            .then(resp => {
                const { data } = resp;
                console.log("Good request", data);
                setList(data);
            })
            .catch(error => {
                console.log("Error server ", error);
            });
    }, []);

    const content = list.map(x => (
        <Col key={x.id} span={6}>
            <Card
                hoverable
                // style={{ width: 240 }}
                cover={<img alt="example" src={`${APP_ENV.BASE_URL}/images/${x.image}`} />}
            >
                <Meta title={x.name} description={x.description} />
            </Card>
        </Col>
    ));
    return (
      <>
          <h1>Hello</h1>
          <Row gutter={16}>
              {content}
          </Row>
      </>
    );
}

export default CategoryListPage;