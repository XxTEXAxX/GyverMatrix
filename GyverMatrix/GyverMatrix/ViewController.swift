//
//  ViewController.swift
//  GyverMatrix
//
//  Created by Андрей Рыбалкин on 03.12.2022.
//

import UIKit
import Network

class FirstViewController: UIViewController {
    
    var host: NWEndpoint.Host = "192.168.4.1"
    var port: NWEndpoint.Port = 2390

    var connection: NWConnection?

    override func viewDidLoad() {
        super.viewDidLoad()
        
        ipTextField.delegate = self
        portTextField.delegate = self
        
        view.backgroundColor = .lightGray
        // Do any additional setup after loading the view.
        
        view.addSubview(stackView)
    }
    
    private lazy var ipTextField: UITextField = {
        let tf = UITextField()
        tf.placeholder = "IP adress"
        tf.borderStyle = .line
        tf.layer.borderWidth = 1
        tf.layer.borderColor = UIColor.black.cgColor
        return tf
    }()

    private lazy var portTextField: UITextField = {
        let tf = UITextField()
        tf.placeholder = "Port"
        tf.borderStyle = .line
        tf.layer.borderWidth = 1
        tf.layer.borderColor = UIColor.black.cgColor
        return tf
    }()
    
    private lazy var connectButton: UIButton = {
        let button = UIButton()
        button.backgroundColor = .systemBlue
        button.setTitle("Connect", for: .normal)
        button.setTitleColor(UIColor.white, for: .normal)
        button.addTarget(self, action: #selector(connect), for: .touchUpInside)
        return button
    }()
    
    private lazy var sendButton: UIButton = {
        let button = UIButton()
        button.backgroundColor = .systemBlue
        button.setTitle("Send", for: .normal)
        button.setTitleColor(UIColor.white, for: .normal)
        button.addTarget(self, action: #selector(sendToEsp), for: .touchUpInside)
        return button
    }()


    private lazy var stackView: UIStackView = {
        let stack = UIStackView(frame: CGRect(x: 0, y: 0, width: self.view.bounds.width - 50, height: 300))
        stack.center = self.view.center
        stack.axis = .vertical
        stack.spacing = 8
        stack.distribution = .fillEqually
        stack.addArrangedSubview(ipTextField)
        stack.addArrangedSubview(portTextField)
        stack.addArrangedSubview(connectButton)
        stack.addArrangedSubview(sendButton)

       return stack
    }()
    
    @objc func sendToEsp() {
        sendUDP("$8 1 10 1;")
        
    }
    func send(_ payload: Data) {
        connection!.send(content: payload, completion: .contentProcessed({ sendError in
            if let error = sendError {
                NSLog("Unable to process and send the data: \(error)")
            } else {
                NSLog("Data has been sent")
                self.connection!.receiveMessage { (data, context, isComplete, error) in
                    guard let myData = data else { return }
                    NSLog("Received message: " + String(decoding: myData, as: UTF8.self))
                }
            }
        }))
    }
    
    
    func sendUDP(_ content: String) {
        let contentToSendUDP = content.data(using: String.Encoding.utf8)
        self.connection?.send(content: contentToSendUDP, completion: NWConnection.SendCompletion.contentProcessed(({ (NWError) in
            if (NWError == nil) {
                print("Data was sent to UDP")
            } else {
                print("ERROR! Error when data (Type: Data) sending. NWError: \n \(NWError!)")
            }
        })))
    }
    @objc func connect() {
//        connection = NWConnection(host: host, port: port, using: .udp)
        connection = NWConnection(host: host, port: port, using: .udp)

        connection!.stateUpdateHandler = { (newState) in
            switch (newState) {
            case .preparing:
                NSLog("Entered state: preparing")
            case .ready:
                NSLog("Entered state: ready")
            case .setup:
                NSLog("Entered state: setup")
            case .cancelled:
                NSLog("Entered state: cancelled")
            case .waiting:
                NSLog("Entered state: waiting")
            case .failed:
                NSLog("Entered state: failed")
            default:
                NSLog("Entered an unknown state")
            }
        }
        
        connection!.viabilityUpdateHandler = { (isViable) in
            if (isViable) {
                NSLog("Connection is viable")
            } else {
                NSLog("Connection is not viable")
            }
        }
        
        connection!.betterPathUpdateHandler = { (betterPathAvailable) in
            if (betterPathAvailable) {
                NSLog("A better path is availble")
            } else {
                NSLog("No better path is available")
            }
        }
        
        connection!.start(queue: .global())
    }



}

extension FirstViewController: UITextFieldDelegate {
    
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        self.view.endEditing(true)
        return false
    }
    
}

