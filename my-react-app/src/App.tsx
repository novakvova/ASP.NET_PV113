import React from "react";
import {Route, Routes} from "react-router-dom";
import CategoryListPage from "./components/categories/list/CategoryListPage.tsx";
import CategoryCreatePage from "./components/categories/create/CategoryCreatePage.tsx";
import CategoryEditPage from "./components/categories/edit/CategoryEditPage.tsx";
import ContainerDefault from "./components/containers/default/ContainerDefault.tsx";
import RegisterPage from "./components/auth/register/RegisterPage.tsx";
import LoginPage from "./components/auth/login/LoginPage.tsx";
import ProductListPage from "./components/products/list/ProductListPage.tsx";
import ProductCreatePage from "./components/products/create/ProductCreatePage.tsx";

const App: React.FC = () => {

  return (
    <>
        <Routes>
            <Route path="/" element={<ContainerDefault/>}>
                <Route index element={<CategoryListPage />} />
                <Route path="create" element={<CategoryCreatePage />} />
                <Route path='edit/:id' element={<CategoryEditPage />} />
                <Route path={"register"} element={<RegisterPage/>} />
                <Route path={"login"} element={<LoginPage/>} />
                <Route path={"products"}>
                    <Route index element={<ProductListPage/>}/>
                    <Route path={"create"} element={<ProductCreatePage/>}/>
                </Route>
            </Route>
        </Routes>
    </>
  )
}

export default App
