import { useEffect, useState } from "react";
import UrlService from "../../Services/url.service";
import { TableUrl } from "./Types/types";
import tokenService from "../../Services/token.service";
import {
  Button,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
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
  }, [isModalOpen]);

  const displayUrlsTable = () => {
    return (
      <TableContainer sx={{ width: "100%" }}>
        <Table sx={{ width: 650, margin: "auto" }} aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell>Original url</TableCell>
              <TableCell align="right">Short url</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {tableUrls?.map((row) => (
              <TableRow
                key={row.originalUrl}
                sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
              >
                <TableCell component="th" scope="row">
                  {row.originalUrl}
                </TableCell>
                <TableCell align="right">{row.shortUrl}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    );
  };

  return (
    <div className="table-content-wrap">
      {userIsLogged ? (
        <>
          <Button
            sx={{
              backgroundColor: "black",
              color: "white",
              ":hover": { backgroundColor: "rgb(73, 73, 73)" },
              margin: "10px",
            }}
            size="large"
            onClick={() => setIsModalOpen(true)}
          >
            Shorten new url ✂️
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
