import {UploadChangeParam} from 'antd/es/upload';

export const imageConverterToFileArray = (e: UploadChangeParam) => {
    return e?.fileList.map(file => file.originFileObj);
};