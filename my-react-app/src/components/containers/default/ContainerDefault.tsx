import React, { useState } from 'react';
import {
    MenuFoldOutlined,
    MenuUnfoldOutlined,
    UploadOutlined,
    UserOutlined,
    VideoCameraOutlined,
    LogoutOutlined
} from '@ant-design/icons';
import { Layout, Menu, Button, theme } from 'antd';
import {Link, Outlet} from "react-router-dom";
// import {IAuthReducerState} from "../../auth/AuthReducer.ts";
import {useAppSelector} from "../../../app/hooks.ts";
import {MenuItemType} from "antd/es/menu/hooks/useItems";

const { Header, Sider, Content } = Layout;

const ContainerDefault: React.FC = () => {
    const [collapsed, setCollapsed] = useState(false);
    const {
        token: { colorBgContainer, borderRadiusLG },
    } = theme.useToken();

    // const {isAuth, user} = useSelector((redux: any)=>redux.auth as IAuthReducerState);
    const auth = useAppSelector(store=>store.auth);

    console.log("---user is Auth---", auth.isAuth);
    console.log("---user info---", auth.user);

    let items : MenuItemType[] = [];
    if(!auth.isAuth) {
        items.push(
            {
                key: '34',
                icon: <UserOutlined />,
                label: <Link to="/login">Вхід</Link>,
            }
        );
        items.push({
            key: '1',
            icon: <UserOutlined />,
            label: <Link to="/register">Реєстрація</Link>,
        });
    }
    else {
        items.push(
            {
                key: '34',
                icon: <LogoutOutlined />,
                label: "Вихід",
                onClick: () => {
                    console.log("Вихід на сайті")
                }
            }
        );

        items.push(
            {
                key: '34',
                icon: <UserOutlined />,
                label: <Link to="/profile">{auth.user?.name}</Link>,
            }
        );
    }

    return (
        <Layout style={{minHeight:"100vh"}}>
            <Sider trigger={null} collapsible collapsed={collapsed}>
                <div className="demo-logo-vertical" />
                <Menu
                    theme="dark"
                    mode="inline"
                    defaultSelectedKeys={['1']}
                    items={[
                        ...items,
                        {
                            key: '2',
                            icon: <VideoCameraOutlined />,
                            label: <Link to="/">Категорії</Link>,
                        },
                        {
                            key: '3',
                            icon: <UploadOutlined />,
                            label: <Link to="/products">Продукти</Link>,
                        },
                    ]}
                />
            </Sider>
            <Layout>
                <Header style={{ padding: 0, background: colorBgContainer }}>
                    <Button
                        type="text"
                        icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
                        onClick={() => setCollapsed(!collapsed)}
                        style={{
                            fontSize: '16px',
                            width: 64,
                            height: 64,
                        }}
                    />
                </Header>
                <Content
                    style={{
                        margin: '24px 16px',
                        padding: 24,
                        minHeight: 280,
                        background: colorBgContainer,
                        borderRadius: borderRadiusLG,
                    }}
                >
                    <Outlet/>
                </Content>
            </Layout>
        </Layout>
    );
};

export default ContainerDefault;