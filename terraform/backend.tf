terraform {
  backend "s3" {
    bucket       = "amzn-terraform-tfstate"
    key          = "terraform.tfstate"
    region       = "eu-central-1"
    use_lockfile = true
    encrypt      = true
  }
}
