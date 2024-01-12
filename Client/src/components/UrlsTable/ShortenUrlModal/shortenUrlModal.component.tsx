import { Box, Button, Fade, FormLabel, Modal, TextField } from "@mui/material";
import "./shortenUrlModal.component.style.css";
import { Form } from "react-router-dom";
import urlService from "../../../Services/url.service";
import { useEffect, useState } from "react";

export default function ShortenUrlModal(props: {
  open: boolean;
  setOpen: React.Dispatch<React.SetStateAction<boolean>>;
}) {
  const [url, setUrl] = useState<string>();
  const [urlError, setUrlError] = useState<boolean>(false);
  const urlRegexPattern = new RegExp(
    "(https?://(?:www.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9].[^s]{2,}|www.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9].[^s]{2,}|https?://(?:www.|(?!www))[a-zA-Z0-9]+.[^s]{2,}|www.[a-zA-Z0-9]+.[^s]{2,})"
  );

  const handleSubmit = async () => {
    url && (await urlService.CreateShortUlr(url));
    props.setOpen(false);
  };

  useEffect(() => {
    if (url) {
      urlRegexPattern?.test(url) ? setUrlError(false) : setUrlError(true);
    }
  }, [url]);

  return (
    <>
      <Modal
        open={props.open}
        onClose={() => props.setOpen(false)}
        closeAfterTransition
      >
        <Fade in={props.open}>
          <Box className="modal-box">
            <Form className="add-modal-form" onSubmit={handleSubmit}>
              <span className="form-title">Shorten an url</span>
              <TextField
                required
                multiline
                minRows={5}
                maxRows={20}
                error={urlError}
                onChange={(e) => setUrl(e.target.value)}
                label="Input your url"
              />
              <div className="add-modal-button-container">
                <Button type="submit">Submit</Button>
              </div>
            </Form>
          </Box>
        </Fade>
      </Modal>
    </>
  );
}
