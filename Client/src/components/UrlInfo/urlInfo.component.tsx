import { useEffect, useState } from "react";
import "./urlInfo.component.style.css";

import urlService from "../../Services/url.service";
import { Link, useNavigate, useParams } from "react-router-dom";
import { UrlInfoData } from "./Types/types";

function UrlInfo() {
  const [urlInfo, setUrlInfo] = useState<UrlInfoData>();
  const params = useParams();
  const navigate = useNavigate();

  const getUrlInfo = async () => {
    try {
      const result = await urlService.GetUrlInfo(Number(params.id));
      setUrlInfo(result);
    } catch (e) {
      navigate("/");
    }
  };

  useEffect(() => {
    getUrlInfo();
  }, []);

  return (
    <>
      {urlInfo && (
        <div className="info-wrap">
          <div className="info-container">
            <div>{`User who registered this url: ${urlInfo.userName}`}</div>
            <div>{`Url creation date: ${new Date(
              urlInfo.date
            ).toDateString()}`}</div>
            <div>
              {`Original url: `}
              <a href={urlInfo.originalUrl} target="_blank">
                {urlInfo.originalUrl}
              </a>
            </div>
            <div>
              {`Short url: `}
              <a href={urlInfo.shortUrl} target="_blank">
                {urlInfo.shortUrl}
              </a>
            </div>
          </div>
        </div>
      )}
    </>
  );
}

export default UrlInfo;
