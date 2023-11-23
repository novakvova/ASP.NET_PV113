import React from "react";
import {Route, Routes} from "react-router-dom";
import CategoryListPage from "./components/categories/list/CategoryListPage.tsx";
import CategoryCreatePage from "./components/categories/create/CategoryCreatePage.tsx";

const App: React.FC = () => {

  return (
    <>
        <Routes>
            <Route path="/">
                <Route index element={<CategoryListPage />} />
                <Route path="create" element={<CategoryCreatePage />} />
            </Route>
        </Routes>
    </>
  )
}

export default App
