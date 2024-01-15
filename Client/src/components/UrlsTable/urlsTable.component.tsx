import { useEffect, useState } from "react";
import UrlService from "../../Services/url.service";
import { TableUrl } from "./Types/types";
import tokenService from "../../Services/token.service";
import DeleteIcon from "@mui/icons-material/Delete";
import jwt_decode from "jwt-decode";
import {
  Button,
  IconButton,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import ShortenUrlModal from "./ShortenUrlModal/shortenUrlModal.component";
import "./urlsTable.component.style.css";
import { useNavigate } from "react-router-dom";

export default function UrlsTable() {
  const [tableUrls, setTableUrls] = useState<TableUrl[]>();
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const userIsLogged = tokenService.isUserLogged();
  const token = tokenService.getLocalAccessToken();
  token &&
    jwt_decode<{
      name: string;
      family_name: string;
      role: string;
    }>(token);

  const navigate = useNavigate();

  const setUrlData = async () => {
    try {
      let response = await UrlService.GetTableUrlsData();
      setTableUrls(response);
    } catch (e) {
      console.log(e);
    }
  };

  useEffect(() => {
    setUrlData();
  }, [isModalOpen]);

  const handleDelete = async (id: number) => {
    try {
      await UrlService.DeleteUrl(id);
      setUrlData();
    } catch (e) {
      console.log(e);
    }
  };

  const displayUrlsTable = () => {
    return (
      <TableContainer sx={{ width: "100%" }}>
        <Table sx={{ width: 650, margin: "auto" }} aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell align="center">Original url</TableCell>
              <TableCell align="center">Short url</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {tableUrls?.map((url) => (
              <TableRow
                className="custom-table-row"
                key={url.id}
                sx={{
                  "&:last-child td, &:last-child th": { border: 0 },
                }}
                onClick={
                  userIsLogged
                    ? () => {
                        navigate(`${url.id}`);
                      }
                    : undefined
                }
              >
                <TableCell align="left">{url.originalUrl}</TableCell>
                <TableCell align="left">{url.shortUrl}</TableCell>
                <TableCell align="left">
                  {url.canDelete && (
                    <IconButton
                      aria-label="fingerprint"
                      onClick={(e) => {
                        e.stopPropagation();
                        handleDelete(url.id);
                      }}
                    >
                      <DeleteIcon sx={{ color: "red" }} />
                    </IconButton>
                  )}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    );
  };

  return (
    <div className="table-content-wrap">
      {userIsLogged && (
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
          <ShortenUrlModal open={isModalOpen} setOpen={setIsModalOpen} />
        </>
      )}

      {tableUrls && tableUrls?.length > 0 ? (
        displayUrlsTable()
      ) : (
        <h1 className="no-content-text">
          {`No urls available. Let's ${
            !userIsLogged ? "log in and " : ""
          }add them...`}
        </h1>
      )}
    </div>
  );
}
