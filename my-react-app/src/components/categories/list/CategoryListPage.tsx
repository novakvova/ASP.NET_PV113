import {Button, Card, Col, Row} from "antd";
import {useEffect, useState} from "react";
import {ICategoryItem} from "../../../interfaces/categories";
import http_common from "../../../http_common.ts";
import {APP_ENV} from "../../../env";
import {Link, useNavigate} from "react-router-dom";
import { EditOutlined, EllipsisOutlined, SettingOutlined } from '@ant-design/icons';


const CategoryListPage = () => {
    const { Meta } = Card;
    const navigate = useNavigate();

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

                actions={[
                    <SettingOutlined key="setting" />,
                    <EditOutlined key="edit" onClick={() => {
                        navigate(`/edit/${x.id}`);
                        console.log("OnClick edit", x.id);
                    }} />,
                    <EllipsisOutlined key="ellipsis" />,
                ]}
            >
                <Meta title={x.name} description={x.description} />
            </Card>
        </Col>
    ));
    return (
      <>
          <h1>Список категорій</h1>
          <Link to={"/create"}>
              <Button type="primary">
                  Додати
              </Button>
          </Link>
          <Row gutter={16}>
              {content}
          </Row>
      </>
    );
}

export default CategoryListPage;