import {
  Box,
  Button,
  Fade,
  FormControl,
  FormLabel,
  Modal,
  Snackbar,
  TextField,
} from "@mui/material";
import "./shortenUrlModal.component.style.css";
import { Form } from "react-router-dom";
import urlService from "../../../Services/url.service";
import { useState } from "react";

export default function ShortenUrlModal(props: {
  open: boolean;
  setOpen: (a: boolean) => void;
}) {
  const [url, setUrl] = useState<string>("");

  const handleSubmit = () => {
    urlService.CreateShortUlr(url);
  };

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
              <FormControl required className="add-modal-element">
                <FormLabel className="label">Input your url</FormLabel>
                <TextField
                  multiline
                  minRows={5}
                  maxRows={20}
                  onChange={(e) => setUrl(e.target.value)}
                />
              </FormControl>
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
