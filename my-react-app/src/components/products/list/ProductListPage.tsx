import {Link, useSearchParams} from "react-router-dom";
import {Button, Col, Collapse, Form, Input, Pagination, Row, Select} from "antd";
import {useEffect, useState} from "react";
import {IProductData, IProductSearch} from "./types.ts";
import {ICategoryName} from "../../../interfaces/categories";
import http_common from "../../../http_common.ts";
import ProductCard from "./ProductCard.tsx";

const ProductListPage = () => {
    const [searchParams, setSearchParams] = useSearchParams();

    const [formParams, setFormParams] = useState<IProductSearch>({
        categoryId: Number(searchParams.get('categoryId')) || undefined,
        name: searchParams.get('name') || "",
        description: searchParams.get('description') || "",
        page: Number(searchParams.get('page')) || 1,
        pageSize: Number(searchParams.get('pageSize')) || 8
    });

    const [data, setData] = useState<IProductData>({
        list: [],
        pageSize: formParams.pageSize,
        // totalPages: 0,
        pageIndex: formParams.page,
        totalCount: 0
    });

    const [categories, setCategories] = useState<ICategoryName[]>([]);

    const [form] = Form.useForm<IProductSearch>();

    useEffect(() => {
        http_common.get<ICategoryName[]>("/api/categories/names")
            .then(resp => {
                console.log("list categories", resp.data);
                setCategories(resp.data);
            });
    }, []);

    useEffect(() => {
        http_common.get<IProductData>("/api/products",
            {
                params: formParams
            })
            .then(resp => {
                console.log("Get products", resp.data);
                setData((resp.data));
            });
    }, [formParams]);

    const onSubmit = async (values: IProductSearch) => {
        findProducts({
            ...formParams,
            page: 1,
            name: values.name,
            description: values.description,
            categoryId: values.categoryId
        });
    }

    const handlePageChange = (page: number, newPageSize: number) => {
        findProducts({...formParams, page, pageSize: newPageSize});
    };

    const findProducts = (model: IProductSearch) => {
        setFormParams(model);
        updateSearchParams(model);
    }

    const updateSearchParams = (params: IProductSearch) => {
        for (const [key, value] of Object.entries(params)) {
            if (key == "pageSize" && value == 8) {
                searchParams.delete(key);
            } else if (value !== undefined && value !== 0 && value != "") {
                searchParams.set(key, value);
            } else {
                searchParams.delete(key);
            }
        }
        setSearchParams(searchParams);
    };

    const optionsData = [
        ...(categories?.map(item => ({label: item.name, value: item.id})) || []),
        {label: 'Усі', value: 0},
    ];

    const {list, pageSize, pageIndex, totalCount} = data;
    return (
        <>
            <h1>Список продуктів</h1>
            <Link to={"/products/create"}>
                <Button type="primary">
                    Додати
                </Button>
            </Link>

            <Collapse defaultActiveKey={0}>
                <Collapse.Panel key={1} header={"Панель пошуку"}>
                        <Form form={form}
                              onFinish={onSubmit}
                              layout={"vertical"}
                              initialValues={formParams}
                              style={{
                                  minWidth: '100%',
                                  display: 'flex',
                                  flexDirection: 'column',
                                  justifyContent: 'center',
                                  padding: 20,
                              }}
                        >
                            <Form.Item
                                label="Назва"
                                name="name"
                                htmlFor="name"
                            >
                                <Input autoComplete="name"/>
                            </Form.Item>

                            <Form.Item
                                label="Опис"
                                name="description"
                                htmlFor="description"
                            >
                                <Input autoComplete="description"/>
                            </Form.Item>

                            <Form.Item label="Категорія:" name="categoryId">
                                <Select
                                    options={optionsData}
                                    placeholder="Select Category"
                                />
                            </Form.Item>

                            <Row style={{display: 'flex', justifyContent: 'center'}}>
                                <Button style={{margin: 10}} type="primary" htmlType="submit">
                                    Пошук
                                </Button>
                                <Button style={{margin: 10}} htmlType="button" onClick={() => {
                                }}>
                                    Скасувати
                                </Button>
                            </Row>
                        </Form>
                </Collapse.Panel>
            </Collapse>

            <Row style={{marginTop: "10px", width: '100%', display: 'flex', justifyContent: 'center'}}>
                <Pagination
                    showTotal={(total, range) => `${range[0]}-${range[1]} із ${total} записів`}
                    current={pageIndex}
                    pageSize={pageSize}
                    total={totalCount}
                    onChange={handlePageChange}
                    pageSizeOptions={[4, 8, 12, 20]}
                    showSizeChanger
                />
            </Row>

            <Row gutter={16}>
                <Col span={24}>
                    <Row>
                        {list.length === 0 ? (
                            <h2>Список пустий</h2>
                        ) : (
                            list.map((item) =>
                                <ProductCard key={item.id} {...item} />,
                            )
                        )}
                    </Row>
                </Col>
            </Row>

            <Row style={{marginTop: "10px", width: '100%', display: 'flex', justifyContent: 'center'}}>
                <Pagination
                    showTotal={(total, range) => `${range[0]}-${range[1]} із ${total} записів`}
                    current={pageIndex}
                    pageSize={pageSize}
                    total={totalCount}
                    onChange={handlePageChange}
                    pageSizeOptions={[4, 8, 12, 20]}
                    showSizeChanger
                />
            </Row>

        </>
    );
}

export default ProductListPage;