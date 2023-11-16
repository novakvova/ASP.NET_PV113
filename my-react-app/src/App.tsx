import axios from "axios";
import { useEffect, useState } from "react"
import { Card, Col, Row } from 'antd';

interface ICategoryItem {
  id: number,
  name: string,
  image: string,
  description: string
}


const App: React.FC = () => {

  const { Meta } = Card;

  const [list, setList] = useState<ICategoryItem[]>([]);

  useEffect(() => {
    axios.get<ICategoryItem[]>("https://rozetka.itstep.click/api/categories")
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
        cover={<img alt="example" src={`https://rozetka.itstep.click/images/${x.image}`} />}
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
  )
}

export default App
