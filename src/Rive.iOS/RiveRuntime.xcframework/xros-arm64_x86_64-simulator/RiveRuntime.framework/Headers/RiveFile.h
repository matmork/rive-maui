//
//  RiveFile.h
//  RiveRuntime
//
//  Created by Maxwell Talbot on 5/14/21.
//  Copyright © 2021 Rive. All rights reserved.
//

#ifndef rive_file_h
#define rive_file_h

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@class RiveArtboard;
@protocol RiveFileDelegate;
@class RiveFileAsset;
@class RiveFactory;
typedef bool (^LoadAsset)(RiveFileAsset* asset,
                          NSData* data,
                          RiveFactory* factory);

/*
 * RiveFile
 */
@interface RiveFile : NSObject

@property(class, readonly) uint majorVersion;
@property(class, readonly) uint minorVersion;

/// Is the Rive file loaded and ready for use?
@property bool isLoaded;

/// Delegate for calling when a file has finished loading
@property(weak) id delegate;

/// Used to manage url sessions Rive, this is to enable testing.
- (nullable instancetype)initWithByteArray:(NSArray*)bytes
                                   loadCdn:(bool)cdn
                                     error:(NSError**)error;
- (nullable instancetype)initWithByteArray:(NSArray*)bytes
                                   loadCdn:(bool)cdn
                         customAssetLoader:(LoadAsset)customAssetLoader
                                     error:(NSError**)error;

- (nullable instancetype)initWithBytes:(UInt8*)bytes
                            byteLength:(UInt64)length
                               loadCdn:(bool)cdn
                                 error:(NSError**)error;
- (nullable instancetype)initWithBytes:(UInt8*)bytes
                            byteLength:(UInt64)length
                               loadCdn:(bool)cdn
                     customAssetLoader:(LoadAsset)customAssetLoader
                                 error:(NSError**)error;

- (nullable instancetype)initWithData:(NSData*)bytes
                              loadCdn:(bool)cdn
                                error:(NSError**)error;
- (nullable instancetype)initWithData:(NSData*)bytes
                              loadCdn:(bool)cdn
                    customAssetLoader:(LoadAsset)customAssetLoader
                                error:(NSError**)error;

- (nullable instancetype)initWithResource:(NSString*)resourceName
                            withExtension:(NSString*)extension
                                  loadCdn:(bool)cdn
                                    error:(NSError**)error;
- (nullable instancetype)initWithResource:(NSString*)resourceName
                            withExtension:(NSString*)extension
                                  loadCdn:(bool)cdn
                        customAssetLoader:(LoadAsset)customAssetLoader
                                    error:(NSError**)error;

- (nullable instancetype)initWithResource:(NSString*)resourceName
                                  loadCdn:(bool)cdn
                                    error:(NSError**)error;
- (nullable instancetype)initWithResource:(NSString*)resourceName
                                  loadCdn:(bool)cdn
                        customAssetLoader:(LoadAsset)customAssetLoader
                                    error:(NSError**)error;

- (nullable instancetype)initWithHttpUrl:(NSString*)url
                                 loadCdn:(bool)cdn
                            withDelegate:(id<RiveFileDelegate>)delegate;
- (nullable instancetype)initWithHttpUrl:(NSString*)url
                                 loadCdn:(bool)cdn
                       customAssetLoader:(LoadAsset)customAssetLoader
                            withDelegate:(id<RiveFileDelegate>)delegate;

/// Returns a reference to the default artboard
- (RiveArtboard* __nullable)artboard:(NSError**)error;

/// Returns the number of artboards in the file
- (NSInteger)artboardCount;

/// Returns the artboard by its index
- (RiveArtboard* __nullable)artboardFromIndex:(NSInteger)index
                                        error:(NSError**)error;

/// Returns the artboard by its name
- (RiveArtboard* __nullable)artboardFromName:(NSString*)name
                                       error:(NSError**)error;

/// Returns the names of all artboards in the file.
- (NSArray<NSString*>*)artboardNames;

@end

/*
 * Delegate to inform when a rive file is loaded
 */
@protocol RiveFileDelegate <NSObject>
- (BOOL)riveFileDidLoad:(RiveFile*)riveFile error:(NSError**)error;
@end

NS_ASSUME_NONNULL_END

#endif /* rive_file_h */
