import {UploadChangeParam} from "antd/es/upload";

export interface IUploadedFile {
    lastModified: number;
    lastModifiedDate: Date;
    name: string;
    originFileObj: File;
    percent: number;
    size: number;
    thumbUrl: string;
    type: string;
    uid: string;
}

export const imageConverter = (e: UploadChangeParam<File>) => {
    return e?.fileList[0] as File;
};