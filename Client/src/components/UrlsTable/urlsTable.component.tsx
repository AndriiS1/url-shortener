import { useEffect, useState } from "react";
import UrlService from "../../Services/url.service";
import { TableUrl } from "./Types/types";
import tokenService from "../../Services/token.service";
import { Button } from "@mui/material";
import ShortenUrlModal from "./ShortenUrlModal/shortenUrlModal.component";
import "./urlsTable.component.style.css";

export default function UrlsTable() {
  const [tableUrls, setTableUrls] = useState<TableUrl[]>();
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
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
    <div className="table-content-wrap">
      {userIsLogged ? (
        <>
          <Button
            className="add-url-btn"
            size="large"
            onClick={() => setIsModalOpen(true)}
          >
            Shorten new url
          </Button>
          {tableUrls && tableUrls?.length > 0 ? (
            displayUrlsTable()
          ) : (
            <h1 className="no-content-text">
              {"No urls available. Let's add them..."}
            </h1>
          )}
          <ShortenUrlModal open={isModalOpen} setOpen={setIsModalOpen} />
        </>
      ) : (
        <h1 className="no-content-text">
          {"No urls available. Let's log in and add them..."}
        </h1>
      )}
    </div>
  );
}
