import { useEffect, useState } from "react";
import "./urlInfo.component.style.css";

import urlService from "../../Services/url.service";
import { useParams } from "react-router-dom";
import { UrlInfoData } from "./Types/types";

function UrlInfo() {
  const [urlInfo, setUrlInfo] = useState<UrlInfoData>();
  const params = useParams();

  const getUrlInfo = async () => {
    if (params) {
      const result = await urlService.GetUrlInfo(Number(params.id));
      setUrlInfo(result);
    }
  };

  useEffect(() => {
    getUrlInfo();
  }, []);

  var options = {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric",
  };

  return urlInfo ? (
    <>
      <div>{`User who registered: ${urlInfo.userName}`}</div>
      <div>{`Url date: ${new Date(urlInfo.date).toDateString()}`}</div>
      <div>{`Original url date: ${urlInfo.originalUrl}`}</div>
      <div>{`Short url ${urlInfo.shortUrl}`}</div>
    </>
  ) : (
    <></>
  );
}

export default UrlInfo;
