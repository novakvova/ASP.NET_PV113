import {Button, Form, Input, message, Row} from 'antd';
import http_common from "../../../http_common.ts";
import {ILogin, ILoginResult, IUserLoginInfo} from "../../../interfaces/auth";
import {jwtDecode} from "jwt-decode";
import {useNavigate} from "react-router-dom";
//import {useAppDispatch} from "../../../app/hooks.ts";
import {AuthReducerActionType} from "../AuthReducer.ts";
import {useDispatch} from "react-redux";
import setAuthToken from "../../../helpers/setAuthToken.ts";

const LoginPage = () => {

    const dispatch = useDispatch();
    const navigator = useNavigate();
    const [form] = Form.useForm<ILogin>();
    const [messageApi, contextHolder] = message.useMessage();

    const onReset = () => {
        onClear();
    };
    const onFinish = async (values: ILogin) => {

        try {
            //begin load
            const result =
                await http_common.post<ILoginResult>("/api/account/login", values);
            //cancel load
            const {token} = result.data;
            const user: IUserLoginInfo = jwtDecode<IUserLoginInfo>(token);

            localStorage.token = token;
            setAuthToken(localStorage.token);
            dispatch({
                    type: AuthReducerActionType.LOGIN_USER,
                    payload: {
                        name: user.name,
                        email: user.email
                    } as IUserLoginInfo
                }
            );
            success();
            onClear();
            navigator("/");
        } catch {
            error();
        }
    };
    const onClear = () => {
        form.resetFields();

    }

    const success = () => {
        messageApi.open({
            type: 'success',
            duration: 10,
            content: 'Категорію успішно створено!',
        });
    };

    const error = () => {
        messageApi.open({
            type: 'error',
            duration: 10,
            content: 'Помилка створення категорії!',
        });
    };

    return (
        <Row gutter={16}>
            {contextHolder}
            <h1>Вхід</h1>
            <Form
                form={form}
                onFinish={onFinish}
                layout="vertical"
                style={{
                    minWidth: '100%',
                    display: 'flex',
                    flexDirection: "column",
                    justifyContent: "center",
                    padding: 20
                }}
            >
                <Form.Item
                    label="Електронна пошта"
                    name="email"
                    htmlFor="email"
                    rules={[
                        {
                            type: 'email',
                            message: 'Формати пошти не правильний!',
                        },
                        {required: true, message: 'Це поле є обов\'язковим!'},
                        {min: 2, message: 'Пошта повинна містити мінімум 2 символи!'}
                    ]}
                >
                    <Input autoComplete="email"/>
                </Form.Item>

                <Form.Item
                    name="password"
                    label="Пароль"
                    rules={[
                        {required: true, message: 'Вкажіть Ваш пароль!',},
                        {min: 6, message: 'Пароль має мати мінімум 6 символів!',},
                    ]}
                    hasFeedback
                >
                    <Input.Password/>
                </Form.Item>

                <Row style={{display: 'flex', justifyContent: 'center'}}>
                    <Button style={{margin: 10}} type="primary" htmlType="submit">
                        Вхід
                    </Button>
                    <Button style={{margin: 10}} htmlType="button" onClick={onReset}>
                        Reset
                    </Button>
                </Row>
            </Form>
        </Row>
    )
}

export default LoginPage;