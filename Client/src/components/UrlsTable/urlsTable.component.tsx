import { useEffect, useState } from "react";
import "./urlsTable.component.style.css";
import UrlService from "../../Services/url.service";
import { TableUrl } from "./Types/types";
import tokenService from "../../Services/token.service";

export default function UrlsTable() {
  const [tableUrls, setTableUrls] = useState<TableUrl[]>();
  const userIsLogged = tokenService.isUserLogged();

  const SetUrlData = async () => {
    let response = await UrlService.GetTableUrlsData();
    setTableUrls(response);
  };

  useEffect(() => {
    SetUrlData();
  }, []);

  const displayUrlsTable = () => {
    return <></>;
  };

  return (
    <div className="urls-wrap">
      {tableUrls && tableUrls?.length > 0 ? (
        displayUrlsTable()
      ) : (
        <h1 className="no-content-text">{`No urls available. Let's ${
          !userIsLogged ? "log in and" : ""
        } add them...`}</h1>
      )}
    </div>
  );
}
